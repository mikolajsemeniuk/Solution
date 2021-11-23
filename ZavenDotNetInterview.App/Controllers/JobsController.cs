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
        private readonly IJobRepository _jobRepository;
        private readonly ILogRepository _logRepository;
        private readonly ZavenDotNetInterviewContext _context;

        public JobsController(ZavenDotNetInterviewContext context, IJobProcessorService jobProcessorService,
                              IJobRepository jobRepository, ILogRepository logRepository)
        {
            _context = context;
            _jobProcessorService = jobProcessorService;
            _jobRepository = jobRepository;
            _logRepository = logRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var jobs = await _jobRepository.GetAllJobsSortedByDateAsync();
            return View(jobs);
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
                var ifJobWithNameExists = await _jobRepository.CheckIfJobWithNameExistsAsync(input.Name);
                if (!ifJobWithNameExists)
                {
                    var job = _jobRepository.CreateNewJob(input.Name, input.DoAfter);
                    _logRepository.CreateNewLog("New", job.Id);
                    if (await _context.SaveChangesAsync() > 0)
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
