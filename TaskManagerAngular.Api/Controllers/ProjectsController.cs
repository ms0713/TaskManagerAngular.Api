using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAngular.Api.Data;
using TaskManagerAngular.Api.Models;

namespace TaskManagerAngular.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    private readonly ISqlConnectionFactory m_SqlConnectionFactory;
    private readonly ApplicationDbContext m_DbContext;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(
        ISqlConnectionFactory sqlConnectionFactory,
        ApplicationDbContext context,
        ILogger<ProjectsController> logger)
    {
        m_SqlConnectionFactory = sqlConnectionFactory;
        m_DbContext = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<Project>> GetAsync()
    {
        using var connection = m_SqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                a.Id AS Id,
                a.Name,
                a.DateOFStart AS DateOFStart,
                a.TeamSize AS TeamSize
            FROM projects AS a
            """;

        var projects = await connection
            .QueryAsync<Project>(
                sql,
                new{});

        return projects.ToList();
    }

    [HttpGet]
    [Route("search")]
    public List<Project> Search(string searchBy, string searchText)
    {
        List<Project> projects = null;
        if (searchBy == "Id")
            projects = m_DbContext.Projects.Where(temp => temp.Id.ToString().Contains(searchText)).ToList();
        else if (searchBy == "Name")
            projects = m_DbContext.Projects.Where(temp => temp.Name.Contains(searchText)).ToList();
        if (searchBy == "DateOfStart")
            projects = m_DbContext.Projects.Where(temp => temp.DateOfStart.ToString().Contains(searchText)).ToList();
        if (searchBy == "TeamSize")
            projects = m_DbContext.Projects.Where(temp => temp.TeamSize.ToString().Contains(searchText)).ToList();

        return projects;
    }

    [HttpPost]
    public async Task<Project> PostAsync([FromBody] Project project)
    {
        m_DbContext.Projects.Add(project);
        m_DbContext.SaveChanges();

        return project;
    }

    [HttpPut]
    public async Task<Project?> PutAsync([FromBody] Project project, CancellationToken cancellationToken)
    {
        Project? existingProject = await m_DbContext.Projects
            .FirstOrDefaultAsync(temp => temp.Id == project.Id, cancellationToken);

        if (existingProject is not null)
        {
            existingProject.Name = project.Name;
            existingProject.DateOfStart = project.DateOfStart;
            existingProject.TeamSize = project.TeamSize;
            m_DbContext.SaveChanges();
            return existingProject;
        }
        else
        {
            return null;
        }
    }

    [HttpDelete]
    public async Task<int> DeleteAsync(int projectId, CancellationToken cancellationToken)
    {
        Project? project = await m_DbContext.Projects
            .FirstOrDefaultAsync(temp => temp.Id == projectId, cancellationToken);

        if (project is not null)
        {
            m_DbContext.Remove(project);
            m_DbContext.SaveChanges();
            return projectId;
        }
        else
        {
            return -1;
        }
    }
}
