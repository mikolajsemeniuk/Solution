using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZavenDotNetInterview.App.DataContext;
using ZavenDotNetInterview.App.Interfaces;
using ZavenDotNetInterview.App.Models;

namespace ZavenDotNetInterview.App.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobProcessorService _jobProcessorService;
        private readonly IJobService _jobService;
        private readonly IJobRepository _jobRepository;
        private readonly ILogRepository _logRepository;

        public JobsController(IJobProcessorService jobProcessorService, IJobService jobService,
                              IJobRepository jobRepository, ILogRepository logRepository)
        {
            _jobProcessorService = jobProcessorService;
            _jobService = jobService;
            _jobRepository = jobRepository;
            _logRepository = logRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await _jobRepository.GetAllJobsSortedByDateAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Process()
        {
            await _jobProcessorService.ProcessJobs();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Job input)
        {
            if (ModelState.IsValid)
            {
                if (!await _jobRepository.CheckIfJobWithNameExistsAsync(input.Name))
                {
                    if (await _jobService.CreateNewJobAsync(input.Name, input.DoAfter))
                    {
                        return RedirectToAction("Index");
                    }
                    ViewBag.Error = "An Error has occured please try again later";
                    return View();
                }
                ViewBag.Error = "Job with this name already exists, please provide other name";
                return View();
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Details(Guid jobId)
        {
            var job = await _jobRepository.GetJobByIdAsync(jobId);
            job.Logs = await _logRepository.GetLogsByIdOrderedByCreatedDate(jobId);
            return View(job);
        }
    }
}
