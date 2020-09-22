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
        public Label[,] generatedLabels;
        public Button[] generatedButtons;

        public string[] CollumnTexts;

        private int CollumnCount;

        public Generator(ref Grid grid, string[] texts, int count)
        {
            baseGrid = grid;
            CollumnCount = count;
            CollumnTexts = texts;
            generatedTextBoxed = new TextBox[CollumnCount, CollumnCount];
            generatedLabels = new Label[CollumnCount, CollumnCount];
            generatedButtons = new Button[CollumnCount];
        }

        public bool GenerateElements(int rowCount)
        {
            generatedTextBoxed = new TextBox[50, 50];
            generatedLabels = new Label[52, 52];
            if (rowCount < 1)
            {
                Logger.WriteRow("Error", $"Не удалось создать элементы интерфейса (count < 1);");
                return false;
            }

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < CollumnCount; j++)
                {
                    CreateTextbox(i, j);
                    CreateLabel(i, j, CollumnTexts[j]);
                }
                CreateButton(i);
            }
            return true;
        }

        public void RemoveElements(int rowCount)
        {
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < CollumnCount; x++)
                {
                    baseGrid.Children.Remove(generatedTextBoxed[x, y]);
                    baseGrid.Children.Remove(generatedLabels[x, y]);
                }
                baseGrid.Children.Remove(generatedButtons[y]);
            }
        }

        public void CreateTextbox(int i, int j)
        {
            generatedTextBoxed[i, j] = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(70 * j + 40, 40 * i, 0, 0),
                Height = 30,
                Width = 30,
                FontFamily = new FontFamily("Microsoft Sans Serif"),
                FontSize = 16,
                TextAlignment = TextAlignment.Center
            };
            baseGrid.Children.Add(generatedTextBoxed[i, j]);
        }

        public void CreateLabel(int i, int j, string text)
        {
            generatedLabels[i, j] = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(70 * j, 40 * i, 0, 0),
                Height = 30,
                Width = 60,
                FontFamily = new FontFamily("Microsoft Sans Serif"),
                FontSize = 16,
                Content = text
            };
            generatedLabels[i, j].BringIntoView();
            baseGrid.Children.Add(generatedLabels[i, j]);
        }

        public void CreateButton(int i)
        {
            generatedButtons[i] = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 40 * i, 10, 0),
                Height = 30,
                Width = 30,
                FontFamily = new FontFamily("Microsoft Sans Serif"),
                FontSize = 16,
                Content = "-"
            };
            generatedButtons[i].BringIntoView();
            baseGrid.Children.Add(generatedButtons[i]);
        }
    }
}
