using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace mathsquiznew
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private int num1, num2, answer, score;
        private string operatorSymbol;
        private DateTime endTime;
        private DateTime startTime;
        private DispatcherTimer questionTimer;
        private DispatcherTimer continuousTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            UpdateScore();
            ScheduleQuestionGeneration();
            SetupContinuousTimer();
            startTime = DateTime.Now;  // Record the start time when the game begins
        }

        private void InitializeGame()
        {
            GenerateQuestion();
            endTime = DateTime.Now.AddSeconds(10);  // Set the initial end time for countdown
            UpdateTimerText();
        }

        private void UpdateScore()
        {
            ScoreText.Text = $"Score: {score}";
        }

        private void SetupContinuousTimer()
        {
            continuousTimer = new DispatcherTimer();
            continuousTimer.Interval = TimeSpan.FromSeconds(1);
            continuousTimer.Tick += (sender, e) =>
            {
                UpdateTimerText();
            };

            continuousTimer.Start();
        } 

        private void ScheduleQuestionGeneration()
        {
            questionTimer = new DispatcherTimer();
            questionTimer.Interval = TimeSpan.FromSeconds(1);
            questionTimer.Tick += (sender, e) =>
            {
                UpdateTimerText();  // Update the timer every second
                if (DateTime.Now >= endTime)
                {
                    questionTimer.Stop();
                    GenerateQuestion();
                    endTime = DateTime.Now.AddSeconds(10);  // Set the new end time for the next countdown
                    questionTimer.Start();
                }
            };

            questionTimer.Start();
        }

        private void GenerateQuestion()
        {
            num1 = random.Next(1, 13);  // Numbers up to 12 for multiplication
            num2 = random.Next(1, 13);

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
                    operatorSymbol = "÷";
                    answer = num1 / num2;
                    break;
            }

            QuestionText.Text = $"{num1} {operatorSymbol} {num2} = ?";
            AnswerInput.Text = "";
        }


        private void AnswerInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitAnswer();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SubmitAnswer()
        {
            if (int.TryParse(AnswerInput.Text, out int userAnswer))
            {
                if (userAnswer == answer)
                {
                    score++;  // Increment the score by one
                    UpdateScore();
                    MessageBox.Show($"Correct! Well done. Your score is now {score}.", "Result");
                    GenerateQuestion();  // Uncomment this line to generate a new question

                    // Reset the timer
                    endTime = DateTime.Now.AddSeconds(10);

                    if (score >= 10)
                    {
                        EndGame();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show($"Incorrect. The correct answer is {answer}. Your score remains {score}.", "Result");
                    GenerateQuestion();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Error");
            }
        }



        private void EndGame()
        {
            TimeSpan elapsedTime = DateTime.Now - startTime;  // Calculate elapsed time
            MessageBox.Show($"Congratulations! You have reached 10 points.\nYour final score is {score}.\nElapsed Time: {elapsedTime:mm\\:ss}", "Game Over");
            Close(); // close the box 
        }

        private void UpdateTimerText()
        {
            TimeSpan remainingTime = endTime - DateTime.Now;
            TimerText.Text = $"Time: {remainingTime:mm\\:ss}";
        }
    }
}
