﻿using ImageTheatre.Data.Models;
using ImageTheatre.Data.Models.DTO;
using ImageTheatre.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageTheatre.API.Controllers;

[Route("api/authors")]
[Authorize]
[ApiController]
public class AuthorsController(IAuthorRepository _authorRepository, 
    ILogger<AuthorsController> logger) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateAuthor(AuthorDto createAuthorDto)
    {
        try
        {
            var author = new Author
            {
                AuthorName = createAuthorDto.AuthorName
            };

            var createdAuthor = await _authorRepository.AddAsync(author);
            return CreatedAtAction(nameof(CreateAuthor), createdAuthor);
        }

        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorDto updateAuthorDto)
    {

        try
        {
            if (id != updateAuthorDto.AuthorID)
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"id in url and form body does not match.");
            }

            var existingauthor = await _authorRepository.GetByIdAsync(id);
            if (existingauthor == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Author with id: {id} does not found");
            }

            existingauthor.AuthorName = updateAuthorDto.AuthorName;

            var updatedAuthor = await _authorRepository.UpdateAsync(existingauthor);

            return Ok(updatedAuthor);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

        [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, $"Author with id: {id} does not found");
        }

        return Ok(author);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        try
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(id);
            if (existingAuthor == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Author with id: {id} does not found");
            }

            await _authorRepository.DeleteAsync(existingAuthor);
            return NoContent();  // return 204
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await _authorRepository.GetAllAsync();
        return Ok(authors);
    }

}