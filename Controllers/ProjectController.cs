using Bl.Service;
using Bl.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Entities;

namespace taskk.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _ProjectService;
        private readonly ITaskKService _TaskItemService;

        public ProjectController(IProjectService ProjectService, ITaskKService TaskItemService)
        {
            _ProjectService = ProjectService;
            _TaskItemService = TaskItemService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var projects = await _ProjectService.GetAllProjectsAsync();
            ViewBag.TaskItems = new SelectList(await _TaskItemService.GetAllTasksAsync() , "Id", "Name");

            return View(projects);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _ProjectService.GetProjectByIdAsync(id);
                     if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.TaskItems = new SelectList(await _TaskItemService.GetAllTasksAsync(), "Id", "Name");
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                await _ProjectService.CreateProjectAsync(project);

                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _ProjectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewBag.TaskItems = new SelectList(await _TaskItemService.GetAllTasksAsync(), "Id", "Name", project.ProjectId);

            return View(project);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _ProjectService.UpdateProjectAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _ProjectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Deleteproject(int id)
        {
            await _ProjectService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
