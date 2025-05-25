using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]

[ApiController]
[Route("api/[controller]")]

public class UsersController(IUserRepository userRepository, IMapper mapper,
IPhotoService PhotoService) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();
        return Ok(users);

    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        if (user == null) return NotFound();
        return user;

    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("could not find user");
        mapper.Map(memberUpdateDto, user);
        userRepository.Update(user);
        if (await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to update the user");


    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("cannot Update user");

        var result = await PhotoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo);
        if (await userRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, mapper.Map<PhotoDto>(photo));
        return BadRequest("Problem Adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("Cannot Use this as a main photo");

        var curretMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (curretMain != null) curretMain.IsMain = false;
        photo.IsMain = true;

        if (await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Problem saving Main photo");

    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (User == null) return BadRequest("User not found");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

        if (photo.PublicId != null)
        {
            var result = await PhotoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);

        }
        user.Photos.Remove(photo);
        if (await userRepository.SaveAllAsync()) return Ok();
        return BadRequest("Problem deleting photo");
    }
}
