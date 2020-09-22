using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BugTrackingSystem
{
    //генератор UI элементов
    public class Generator
    {
        public Grid baseGrid = new Grid();
        public TextBox[,] generatedTextBoxed;
        public Label[] generatedLabels;
        //public Button[] generatedButtons;

        public string[] CollumnTexts;

        public int CollumnCount;
        public int RowCount;

        public Generator(ref Grid grid, string[] texts, int collumn, int row)
        {
            baseGrid = grid;
            
            CollumnCount = collumn;
            RowCount = row;

            CollumnTexts = texts;
            generatedTextBoxed = new TextBox[RowCount, CollumnCount];
            generatedLabels = new Label[CollumnCount];
            //generatedButtons = new Button[RowCount];
        }

        public bool GenerateElements(int row = -1)
        {
            if (row != -1)
                RowCount = row;

            generatedTextBoxed = new TextBox[CollumnCount, RowCount];
            generatedLabels = new Label[CollumnCount];

            for (int j = 0; j < CollumnCount; j++)
            {
                CreateLabel(j, CollumnTexts[j]);
            }

            for (int y = 0; y < RowCount; y++)
            {
                for (int x = 0; x < CollumnCount; x++)
                {
                    CreateTextbox(x, y);
                }
            }
            return true;
        }

        public void RemoveRow(int y)
        {
            for (int x = 0; x < CollumnCount; x++)
            {
                baseGrid.Children.Remove(generatedTextBoxed[x, y]);
                baseGrid.Children.Remove(generatedLabels[x]);
            }
            //baseGrid.Children.Remove(generatedButtons[y]);

        }

        public void RemoveElements()
        {
            for (int y = 0; y < RowCount; y++)
            {
                for (int x = 0; x < CollumnCount; x++)
                {
                    baseGrid.Children.Remove(generatedTextBoxed[x, y]);
                    baseGrid.Children.Remove(generatedLabels[x]);
                }
                //baseGrid.Children.Remove(generatedButtons[y]);
            }
        }

        public void CreateTextbox(int x, int y)
        {
            generatedTextBoxed[x, y] = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(110 * x , 40 * (y + 1) , 0, 0),
                Height = 30,
                Width = 100,
                FontFamily = new FontFamily("Microsoft Sans Serif"),
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
            };

            if (x == 0)
                generatedTextBoxed[x, y].IsReadOnly = true;


            baseGrid.Children.Add(generatedTextBoxed[x, y]);
        }

        public void CreateLabel(int x, string text)
        {
            generatedLabels[x] = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(105 * x + 30, 0, 0, 0),
                Height = 30,
                Width = 100,
                FontFamily = new FontFamily("Microsoft Sans Serif"),
                FontSize = 16,
                Content = text
            };
            generatedLabels[x].BringIntoView();
            baseGrid.Children.Add(generatedLabels[x]);
        }

        /*
        public void CreateButton(int y)
        {
            generatedButtons[y] = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 40 * y, 10, 0),
                Height = 30,
                Width = 30,
                FontFamily = new FontFamily("Microsoft Sans Serif"),
                FontSize = 16,
                Content = "-"
            };
            generatedButtons[y].BringIntoView();
            generatedButtons[y].Name = $"{y}";
            generatedButtons[y].ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(generatedButtons[y]));
            baseGrid.Children.Add(generatedButtons[y]);
        }

        private void button_click(object sender, RoutedEventArgs e, int y)
        {

        }*/
    }
}
