
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.Services.QuestionServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuestionController(IQuestionService _questionService) : Controller
    {
        

        public async Task<IActionResult> Index()
        {
            var questions = await _questionService.GetAllAsync();

            return View(questions);
        }

        public async Task<IActionResult> Detail(string id)
        {
            await _questionService.MarkAsReadAsync(id);

            var question = await _questionService.GetByIdAsync(id);

            if (question == null)
                return NotFound();

            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Answer(AnswerQuestionDto answerQuestionDto)
        {
            if (string.IsNullOrWhiteSpace(answerQuestionDto.Answer))
            {
                TempData["Error"] = "Lütfen cevap alanını doldurun.";

                return RedirectToAction(nameof(Detail), new
                {
                    id = answerQuestionDto.Id
                });
            }

            await _questionService.AnswerAsync(answerQuestionDto);

            TempData["Success"] = "Cevap başarıyla gönderildi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _questionService.DeleteAsync(id);

            TempData["Success"] = "Soru başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}

