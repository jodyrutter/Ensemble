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
    public partial class MatchingPage : ContentPage
    {
        public ObservableCollection<person> people { get; set; }
        public MatchingPage()
        {
            people = new ObservableCollection<person>();
            var lView = new ListView();
            lView.ItemsSource = people;

            lView.ItemTemplate = new DataTemplate(typeof(ImageCell));
            lView.ItemTemplate.SetBinding(ImageCell.TextProperty, "name");
            lView.ItemTemplate.SetBinding(ImageCell.DetailProperty, "instruments");
            lView.ItemTemplate.SetBinding(ImageCell.ImageSourceProperty, "image");

            Content = lView;

            people.Add(new person() { name = "Steve", instruments = "Guitar", image = "" });
            people.Add(new person() { name = "John", instruments = "Piano", image = "" });
            people.Add(new person() { name = "Tom", instruments = "Flute", image = "" });
            people.Add(new person() { name = "Lucas", instruments = "Recorder", image = "" });
            people.Add(new person() { name = "Tariq", instruments = "Drums", image = "" });
            people.Add(new person() { name = "Jane", instruments = "Singer", image = "" });
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