using DotNet8WebApi.ODataSample.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DotNet8WebApi.ODataSample.Features.Blog;

public class BlogsController : ODataController
{
    private readonly AppDbContext _appDbContext;

    public BlogsController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [EnableQuery]
    public IQueryable<BlogModel> Get()
    {
        return _appDbContext.Blogs;
    }

    [EnableQuery]
    public SingleResult<BlogModel> Get([FromODataUri] int key)
    {
        var result = _appDbContext.Blogs.Where(b => b.BlogId == key);
        return SingleResult.Create(result);
    }

    public async Task<IActionResult> Post([FromBody] BlogModel blog)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _appDbContext.Blogs.AddAsync(blog);
        await _appDbContext.SaveChangesAsync();

        return Created(blog);
    }

    public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] BlogModel blog)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var item = await _appDbContext.Blogs.FindAsync(key);
        if (item is null)
            return NotFound(MessageResource.NotFound);

        if (!string.IsNullOrEmpty(blog.BlogTitle))
        {
            item.BlogTitle = blog.BlogTitle;
        }

        if (!string.IsNullOrEmpty(blog.BlogAuthor))
        {
            item.BlogAuthor = blog.BlogAuthor;
        }

        if (!string.IsNullOrEmpty(blog.BlogContent))
        {
            item.BlogContent = blog.BlogContent;
        }

        int result = await _appDbContext.SaveChangesAsync();

        return result > 0 ? Accepted() : BadRequest(MessageResource.SaveFail);
    }

    public async Task<IActionResult> Delete([FromODataUri] int key)
    {
        if (key <= 0)
            return BadRequest();

        var item = await _appDbContext.Blogs.FindAsync(key);
        if (item is null)
            return NotFound(MessageResource.NotFound);

        _appDbContext.Blogs.Remove(item);
        int result = await _appDbContext.SaveChangesAsync();

        return result > 0 ? Accepted() : BadRequest(MessageResource.DeleteFail);
    }
}