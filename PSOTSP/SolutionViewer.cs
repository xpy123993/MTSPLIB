using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSOTSP
{
    public partial class SolutionViewer : Form
    {
        public class WorkCanvas
        {
            private Bitmap bitmap;
            private Graphics g;

            private Cost cost;
            private Solution solution;
            private ProblemInstance problem;

            public const int CanvasWidth = 100;
            public const int CanvasHeight = 100;

            public int Width, Height;

            public const int SymbolSize = 5;
            public Color[] colors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Cyan, Color.YellowGreen, Color.Purple };

            public String title = "";

            private float cx(double x)
            {
                return (float)(Width / 2 + x * (Width / CanvasWidth));
            }

            private float cy(double y)
            {
                return (float)(Height / 2 - y * (Height / CanvasHeight));
            }

            public WorkCanvas(int Width, int Height, ProblemInstance problem, Solution solution, Cost cost, String title)
            {
                this.problem = problem;
                this.solution = solution;
                this.title = title;
                this.cost = cost;

                this.Width = Width;
                this.Height = Height;
            }

            public string update_text()
            {
                String text = "解决方案摘要\n";
                text += "\n" + cost.tag + ":" + cost.totalCost + "\n\n";
                for (int i = 0; i < problem.m; i++)
                {
                    text += "外卖骑士-" + i + ":\n路径：";
                    for (int j = 0; j < solution.job_ids[i].Count; j++)
                    {
                        if (j != 0) text += "->";
                        JobDetails jobDetails = problem.job_details[solution.job_ids[i][j]];
                        text += String.Format("({0:0.00},{1:0.00})", jobDetails.x, jobDetails.y);
                    }
                    text += "\n路径代价：" + cost.costs[i] + "\n\n";
                }

                return text.Replace("\n", "\r\n");
            }

            private void drawAxis()
            {
                Pen pen = new Pen(Color.Gray);

                int min_x = -CanvasWidth / 2;
                int max_x = CanvasWidth / 2;
                int min_y = -CanvasHeight / 2;
                int max_y = CanvasHeight / 2;

                for (int y = min_y; y <= max_y; y += 10)
                {
                    g.DrawLine(pen, cx(min_x), cy(y), cx(max_x), cy(y));
                }
                for (int x = min_x; x <= max_x; x += 10)
                {
                    g.DrawLine(pen, cx(x), cy(min_y), cx(x), cy(max_y));
                }
            }

            public Bitmap getBitmap()
            {
                bitmap = new Bitmap(this.Width, this.Height);
                g = Graphics.FromImage(bitmap);

                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                g.Clear(Color.White);
                drawAxis();

                for (int i = 0; i < problem.m; i++)
                    draw_worker(i);

                return bitmap;
            }

            private void draw_worker(int workerID)
            {
                List<int> job_ids = solution.job_ids[workerID];
                if (job_ids.Count == 0) return;

                Brush brush = new SolidBrush(Color.Black);
                Pen pen = new Pen(Color.FromArgb(250, colors[workerID % colors.Length]));

                g.FillEllipse(brush, cx(0) - SymbolSize / 2, cy(0) - SymbolSize / 2, SymbolSize, SymbolSize);
                PointF[] points = new PointF[job_ids.Count + 2];

                points[0] = new PointF(cx(0), cy(0));
                for (int i = 0; i < job_ids.Count; i++)
                {
                    JobDetails jobDetails = problem.job_details[job_ids[i]];
                    points[i + 1] = new PointF(cx(jobDetails.x), cy(jobDetails.y));
                }
                points[job_ids.Count + 1] = new PointF(cx(0), cy(0));
                g.DrawLines(pen, points);
                for (int i = 0; i < job_ids.Count; i++)
                {
                    JobDetails jobDetails = problem.job_details[job_ids[i]];
                    g.FillEllipse(brush, cx(jobDetails.x) - SymbolSize / 2, cy(jobDetails.y) - SymbolSize / 2, SymbolSize, SymbolSize);
                }


            }
        }
        

        public SolutionViewer(ProblemInstance problem, Solution solution, Cost cost, String title)
        {
            InitializeComponent();

            WorkCanvas wc = new WorkCanvas(pictureBox1.Width, pictureBox1.Height, problem, solution, cost, title);
        }


        private WorkCanvas wc;



        private void SolutionViewer_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = wc.getBitmap();
            textBox1.Text = wc.update_text();
        }

        
    }
}
