using System.Collections.Generic;
using System.Web.Mvc;
using SudokuSolver.lib;
using SudokuSolver.web.Models;

namespace SudokuSolver.web.Controllers
{
  public class HomeController : Controller
  {
    private readonly SudokuSolverCs _service;
    private List<string> _solvedModels;

    public HomeController()
    {
      _service = new SudokuSolverCs();
      _solvedModels = new List<string>
            {
                "85...24..72......9..4.........1.7..23.5...9...4...........8..7..17..........36.4.",
                "4.....8.5.3..........7......2.....6.....8.4......1.......6.3.7.5..2.....1.4......",
                "52...6.........7.13...........4..8..6......5...........418.........3..2...87.....",
                "1....7.9..3..2...8..96..5....53..9...1..8...26....4...3......1..4......7..7...3.."
            };
    }

    public ActionResult Index()
    {
      var model = new BoardUIModel(_service.get_board(null)) { OriginalInput = _solvedModels };

      return View(model);
    }

    [HttpPost]
    public ActionResult Board(string input)
    {
      AddInputToSolvedModels(input);
      var model = new BoardUIModel(_service.get_board(input)) { OriginalInput = _solvedModels };
      return View("Index", model);
    }

    [HttpPost]
    public ActionResult Solve(string[] values)
    {
      values = CleanValues(values);
      var input = string.Join("", values);
      AddInputToSolvedModels(input);
      var model = new BoardUIModel(_service.search(_service.parse_grid(input))) { OriginalInput = _solvedModels };

      return View("Index", model);
    }

    private string[] CleanValues(string[] values)
    {
      for (var i = 0; i < values.Length; i++)
      {
        if (values[i].Trim() == "") values[i] = ".";
      }
      return values;
    }

    private void AddInputToSolvedModels(string input)
    {
      if (_solvedModels.Contains(input)) return;

      _solvedModels.Add(input);
    }
  }
}
