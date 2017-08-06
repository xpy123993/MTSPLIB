using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSOTSP
{
    class Program
    {

        static SolutionViewer create_viewer(ProblemInstance problem, Optimizer optimizer, Evaluator evaluator)
        {
            Solution solution = optimizer.minimize(problem, evaluator);
            SolutionViewer solutionViewer = new SolutionViewer(problem, solution, evaluator.evaluate(problem, solution), solution.tag);
            return solutionViewer;
        }


        static void Main(string[] args)
        {

            ProblemInstance problem = ProblemFactory.getRandomProblemInstance(3, 40, 40, 50);
            Evaluator evaluator = EvaluatorFactory.getMakespanEvaluator();

            List<SolutionViewer> viewers = new List<SolutionViewer>();

            viewers.Add(create_viewer(problem, OptimizerFactory.getGeneticAlgorithmOptimizer(), evaluator));
            viewers.Add(create_viewer(problem, OptimizerFactory.getPSOOptimizer(), evaluator));
            viewers.Add(create_viewer(problem, OptimizerFactory.getScannerOptimizer(), evaluator));
            viewers.Add(create_viewer(problem, OptimizerFactory.getGreedyOptimizer(), evaluator));
            viewers.Add(create_viewer(problem, OptimizerFactory.getImprovedGreedyOptimizer(), evaluator));
            Controller controller = new Controller(viewers);

            controller.ShowDialog();
        }
    }
}
