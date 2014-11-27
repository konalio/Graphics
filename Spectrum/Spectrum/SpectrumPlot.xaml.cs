using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace Spectrum {
    /// <summary>
    /// Interaction logic for SpectrumPlot.xaml
    /// </summary>
    public partial class SpectrumPlot {
        //Lines on plot to choose desired values precisely
        private readonly Line _verticalLine = new Line { X1 = 0, Y1 = Constants.PlotHeight, X2 = 0, Y2 = Constants.PlotHeight, Stroke = Brushes.Cyan };
        private readonly Line _horizontalLine = new Line { X1 = 0, Y1 = 0, X2 = 0, Y2 = 0, Stroke = Brushes.Cyan };

        private bool _plotIsEdited;
        private readonly Timer _editingTimer = new Timer(10);

        public List<Factor> Factors { get; set; }

        public SpectrumPlot() {
            InitializeComponent();
            _editingTimer.Elapsed += (sender, args) => HandleSpectrumEditing();
            _editingTimer.Start();
        }

        private void PlotAreaCanvas_MouseMove(object sender, MouseEventArgs e) {
            var currentMousePoint = e.GetPosition(this);

            currentMousePoint.X -= 20;
            currentMousePoint.Y -= 10;

            MovePlotHelperLines(currentMousePoint);

            if (!_plotIsEdited) return;

            //ToDo przypadki brzegowe
            HandleSpectrumEditing();
        }

        private void HandleSpectrumEditing() {
            if (!_plotIsEdited) return;

            var currentMousePoint = Mouse.GetPosition(PlotAreaCanvas);
            var mouseXAsInt = (int)currentMousePoint.X;
            var mouseYAsInt = (int)currentMousePoint.Y;

            var index = mouseXAsInt / 2;
            var newIntensity = ((double)(Constants.PlotHeight - mouseYAsInt)) / Constants.MaxIntensityInPixels;

            if (index >= Factors.Count) return;

            var chosenFactor = Factors[index];
            chosenFactor.Intensity = newIntensity;

            Canvas.SetTop(chosenFactor.DrawnFactor, currentMousePoint.Y);
            Canvas.SetLeft(chosenFactor.DrawnFactor, currentMousePoint.X);

            chosenFactor.LeftLine.X2 = currentMousePoint.X;
            chosenFactor.LeftLine.Y2 = currentMousePoint.Y;

            chosenFactor.RightLine.X1 = currentMousePoint.X;
            chosenFactor.RightLine.Y1 = currentMousePoint.Y;
        }

        private void MovePlotHelperLines(Point currentMousePoint) {
            if (currentMousePoint.X < 0 || currentMousePoint.X > 640 || currentMousePoint.Y < 10 ||
                currentMousePoint.Y > 510)
                return;

            MoveVerticalLine(currentMousePoint);
            MoveHorizontalLine(currentMousePoint);
        }

        private void MoveHorizontalLine(Point currentMousePoint) {
            _horizontalLine.X2 = currentMousePoint.X;
            _horizontalLine.Y1 = currentMousePoint.Y;
            _horizontalLine.Y2 = currentMousePoint.Y;
        }

        private void MoveVerticalLine(Point currentMousePoint) {
            _verticalLine.X2 = currentMousePoint.X;
            _verticalLine.X1 = currentMousePoint.X;
            _verticalLine.Y2 = currentMousePoint.Y;
        }

        private void PlotAreaCanvas_Loaded(object sender, RoutedEventArgs e) {
            PlotAreaCanvas.Children.Add(_verticalLine);
            PlotAreaCanvas.Children.Add(_horizontalLine);

            Factors = new List<Factor>();

            for (var i = Constants.MinWavelength; i <= Constants.MaxWavelength; i++) {
                Factors.Add(new Factor(i));
            }

            for (var i = 0; i < Factors.Count; i++) {
                var factorRect = new Rectangle { Width = 2, Height = 2, Fill = Brushes.Black };
                Canvas.SetBottom(factorRect, 0);
                Canvas.SetLeft(factorRect, Factors[i].FactorPoint.X);
                PlotAreaCanvas.Children.Add(factorRect);
                Factors[i].DrawnFactor = factorRect;

                var leftLine = i == 0
                    ? new Line {
                        X1 = 0,
                        Y1 = Constants.PlotHeight,
                        X2 = Factors[i].FactorPoint.X,
                        Y2 = Factors[i].FactorPoint.Y,
                        Stroke = Brushes.Black
                    }
                    : Factors[i - 1].RightLine;

                var rightLine = new Line {
                    X2 = i == Factors.Count - 1 ? Constants.PlotWidth : Factors[i + 1].FactorPoint.X,
                    Y2 = i == Factors.Count - 1 ? Constants.PlotHeight : Factors[i + 1].FactorPoint.Y,
                    X1 = Factors[i].FactorPoint.X,
                    Y1 = Factors[i].FactorPoint.Y,
                    Stroke = Brushes.Black
                };

                if (i == 0)
                    PlotAreaCanvas.Children.Add(leftLine);

                PlotAreaCanvas.Children.Add(rightLine);

                Factors[i].LeftLine = leftLine;
                Factors[i].RightLine = rightLine;
            }
        }

        private void PlotAreaCanvas_MouseDown(object sender, MouseButtonEventArgs e) {
            _plotIsEdited = true;
        }

        private void PlotAreaCanvas_MouseUp(object sender, MouseButtonEventArgs e) {
            _plotIsEdited = false;
        }
    }
}
