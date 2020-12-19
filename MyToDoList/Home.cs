using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using MyToDoList.Models;

namespace MyToDoList
{
    public class MainPage : ContentPage
    {
        ListView list;
        public MainPage()
        {
            Title = "My ToDo List";
            var toolbarItem = new ToolbarItem
            {
                Text = "Add New",               
            };
            toolbarItem.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new ItemsEditPage
                {
                    BindingContext = new TodoItem()
                });
            };

            ToolbarItems.Add(toolbarItem);

            list = new ListView
            {
                Margin = new Thickness(20),
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label
                    {
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        FontSize = 20
                    };
                    label.SetBinding(Label.TextProperty, "Name");

                    var tick = new Image
                    {
                        Source = ImageSource.FromFile("Images/Tick.png"),
                        HorizontalOptions = LayoutOptions.End
                    };
                    tick.SetBinding(VisualElement.IsVisibleProperty, "Done");

                    var stackLayout = new StackLayout
                    {
                        Margin = new Thickness(20, 0, 0, 0),
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children = { label, tick }
                    };

                    return new ViewCell { View = stackLayout };
                })
            };
            list.ItemSelected += async (sender, e) =>
            {

                if (e.SelectedItem != null)
                {
                    await Navigation.PushAsync(new ItemsEditPage
                    {
                        BindingContext = e.SelectedItem as TodoItem
                    });
                }
            };

            Content = list;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            list.ItemsSource = await App.Database.GetItemsAsync();
        }
    }
}
