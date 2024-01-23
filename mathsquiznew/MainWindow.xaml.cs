using System;
using System.Windows;
using System.Windows.Threading;

namespace mathsquiznew
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private int num1, num2, answer, score;
        private string operatorSymbol;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            GenerateQuestion();
            StartTimer();
        }

        private void UpdateScore()
        {
            ScoreText.Text = $"Score: {score}";
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timer != null && timer.IsEnabled)
            {
                if (timer.Interval.TotalSeconds == 5)
                {
                    timer.Stop();
                    // MessageBox.Show($"Time's up! The correct answer is {answer}. Your score remains {score}.", "Result");
                    // commented out in case 
                    GenerateQuestion();
                    StartTimer();
                }
                else
                {
                    timer.Interval = timer.Interval.Add(TimeSpan.FromSeconds(1));
                }
            }
        }

        private void StartTimer()
        {
            timer.Start();
            timer.Interval = TimeSpan.FromSeconds(1);
        }

        private void GenerateQuestion()
        {
            num1 = random.Next(1, 11);
            num2 = random.Next(1, 11);

            switch (random.Next(1, 5))
            {
                case 1:
                    operatorSymbol = "+";
                    answer = num1 + num2;
                    break;
                case 2:
                    operatorSymbol = "-";
                    answer = num1 - num2;
                    break;
                case 3:
                    operatorSymbol = "x";
                    answer = num1 * num2;
                    break;
                case 4:
                    operatorSymbol = "/";
                    answer = num1 / num2;
                    break;
            }

            QuestionText.Text = $"{num1} {operatorSymbol} {num2} = ?";
            AnswerInput.Text = "";
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AnswerInput.Text, out int userAnswer))
            {
                timer.Stop();

                if (userAnswer == answer)
                {
                    MessageBox.Show($"Correct! Well done. Your score is now {++score}.", "Result");
                }
                else
                {
                    MessageBox.Show($"Incorrect. The correct answer is {answer}. Your score remains {score}.", "Result");
                }

                GenerateQuestion();
                StartTimer();
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Error");
            }

        }
    }
}
