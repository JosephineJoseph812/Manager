using Bl.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Entities;

namespace taskk.Controllers
{
    public class TaskKController : Controller
    {
        private readonly ITaskKService _TaskItemService;
        private readonly IProjectService _projectService;


        public TaskKController(ITaskKService TaskItemService, IProjectService projectService)
        {
            _TaskItemService = TaskItemService;
            _projectService = projectService;
        }
        public async Task<IActionResult> Index()
        {
            var TaskItems = await _TaskItemService.GetAllTasksAsync();
            return View(TaskItems);
        }
        public async Task<IActionResult> Details(int id)
        {
            var TaskItem = await _TaskItemService.GetTasksByProjectIdAsync(id);
            if (TaskItem == null)
            {
                return NotFound();
            }

            return View(TaskItem);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskK TaskItem)
        {
            if (ModelState.IsValid)
            {
                await _TaskItemService.CreateTaskAsync(TaskItem);
                return RedirectToAction(nameof(Index));
            }
            return View(TaskItem);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var TaskItem = await _TaskItemService.GetTaskByIdAsync(id);
            if (TaskItem == null)
            {
                return NotFound();
            }
            return View(TaskItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskK TaskItem)
        {
            if (id != id) return NotFound();

            if (ModelState.IsValid)
            {
                await _TaskItemService.UpdateTaskAsync(TaskItem);
                return RedirectToAction(nameof(Index));
            }
            return View(TaskItem);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var TaskItem = await _TaskItemService.GetTaskByIdAsync(id);
            if (TaskItem == null) return NotFound();
            return View(TaskItem);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            await _TaskItemService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
