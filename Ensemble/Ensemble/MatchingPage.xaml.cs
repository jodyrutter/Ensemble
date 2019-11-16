using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ensemble
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchingPage : ContentView
    {
        public ObservableCollection<person> people { get; set; }
        ListView lView = new ListView()
        {
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Color.White,
            HasUnevenRows = true,
        };

        public MatchingPage()
        {
            people = new ObservableCollection<person>();

            DataTemplate ListDataTemplate = new DataTemplate(() =>
            {
                SwipeGestureGrid gridData = new SwipeGestureGrid()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 60,
                    RowDefinitions =
                        {
                            new RowDefinition { },
                        },
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },                                                   //Put Cells Data here
                            new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) },                                                   //Put Cells Data here
                        }
                };
                Grid gridBase = new Grid()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 60,
                    RowDefinitions =
                    {
                        new RowDefinition { },
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute)},   //Button for Cells here
                        new ColumnDefinition {  },                                                   //Put Cells Data here
                        new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute)},   //Button for Cells here
                    },
                };

                Button btnCellDelete = new Button() { Text = "No", BackgroundColor = Color.Red };
                Button btnCellApprove = new Button() { Text = "Yes", BackgroundColor = Color.Green };

                var nameLabel = new Label { FontAttributes = FontAttributes.Bold };
                var insLabel = new Label();
                var imageLabel = new Image();

                nameLabel.SetBinding(Label.TextProperty, "name");
                insLabel.SetBinding(Label.TextProperty, "instruments");
                imageLabel.SetBinding(Image.SourceProperty, "image");

                gridData.Children.Add(nameLabel, 1, 0);
                gridData.Children.Add(insLabel, 2, 0);
                gridData.Children.Add(imageLabel, 0, 0);

                gridBase.Children.Add(btnCellApprove, 0, 0);
                gridBase.Children.Add(gridData, 1, 0);
                gridBase.Children.Add(btnCellDelete, 2, 0);

                gridData.SwipeLeft += GridTemplate_SwipeLeft;
                gridData.SwipeRight += GridTemplate_SwipeRight; ;
                gridData.Tapped += GridTemplate_Tapped; ;
                btnCellDelete.Clicked += BtnCellDelete_Clicked; ;

                return new ViewCell
                {
                    View = gridBase,
                    Height = 60,
                };
            });

            people.Add(new person() { name = "Steve", instruments = "Guitar", image = "" });
            people.Add(new person() { name = "John", instruments = "Piano", image = "" });
            people.Add(new person() { name = "Tom", instruments = "Flute", image = "" });
            people.Add(new person() { name = "Lucas", instruments = "Recorder", image = "" });
            people.Add(new person() { name = "Tariq", instruments = "Drums", image = "" });
            people.Add(new person() { name = "Jane", instruments = "Singer", image = "" });

            lView.ItemTemplate = ListDataTemplate;
            lView.ItemsSource = people;
            Content = lView;
        }

        private void GridTemplate_Tapped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GridTemplate_SwipeLeft(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid)
                {
                    var templateGrid = ((SwipeGestureGrid)sender).Parent;
                    if (templateGrid != null && templateGrid is Grid)
                    {
                        var CellTemplateGrid = (Grid)templateGrid;
                        if(CellTemplateGrid.ColumnDefinitions[0].Width.Value == 120)
                            CellTemplateGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Absolute);
                        else
                            CellTemplateGrid.ColumnDefinitions[2].Width = new GridLength(120, GridUnitType.Absolute);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void GridTemplate_SwipeRight(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid)
                {
                    var templateGrid = ((SwipeGestureGrid)sender).Parent;
                    if (templateGrid != null && templateGrid is Grid)
                    {
                        var CellTemplateGrid = (Grid)templateGrid;
                        if (CellTemplateGrid.ColumnDefinitions[2].Width.Value == 120)
                            CellTemplateGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Absolute);
                        else
                            CellTemplateGrid.ColumnDefinitions[0].Width = new GridLength(120, GridUnitType.Absolute);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void BtnCellDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button)
                {
                    var templateGrid = ((Button)sender);
                    //templateGrid.Parent = gridBase
                    //templateGrid.Parent.Parent = cell
                    if (templateGrid.Parent != null && templateGrid.Parent.Parent != null && templateGrid.Parent.Parent.BindingContext != null)
                    {
                        var deletedate = templateGrid.Parent.Parent.BindingContext;
                        people.Remove((person)deletedate);
                        lView.ItemsSource = null;
                        lView.ItemsSource = people;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public class person
        {
            public string name { get; set; }
            public string instruments { get; set; }
            public string image { get; set; }
            public person()
            {

            }
        }
    }
}
