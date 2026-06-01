using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace List3
{
    public partial class MainWindow : Window
    {
        public class ItemInformation : INotifyPropertyChanged
        {
            private int itemID;
            private string itemName;
            private string itemDescription;
            private string itemPrice;

            public int ItemID
            {
                get => itemID;
                set { itemID = value; OnPropertyChanged(nameof(ItemID)); }
            }

            public string ItemName
            {
                get => itemName;
                set { itemName = value; OnPropertyChanged(nameof(ItemName)); }
            }

            public string ItemDescription
            {
                get => itemDescription;
                set { itemDescription = value; OnPropertyChanged(nameof(ItemDescription)); }
            }

            public string ItemPrice
            {
                get => itemPrice;
                set { itemPrice = value; OnPropertyChanged(nameof(ItemPrice)); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        private List<ItemInformation> items = new List<ItemInformation>();
        private List<ItemInformation> cartItems = new List<ItemInformation>();

        public MainWindow()
        {
            InitializeComponent();

            items.Add(new ItemInformation
            {
                ItemID = 01,
                ItemName = "Notebook",
                ItemDescription = "Used for writing notes",
                ItemPrice = "50.00"
            });

            items.Add(new ItemInformation
            {
                ItemID = 02,
                ItemName = "Ballpen",
                ItemDescription = "Blue ink pen",
                ItemPrice = "15.00"
            });

            RefreshGrids();
        }

        private void RefreshGrids()
        {
            ItemGrid.ItemsSource = null;
            ItemGrid.ItemsSource = items;

            CartGrid.ItemsSource = null;
            CartGrid.ItemsSource = cartItems;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ItemID.Text) ||
                    string.IsNullOrWhiteSpace(ItemName.Text) ||
                    string.IsNullOrWhiteSpace(ItemDescription.Text) ||
                    string.IsNullOrWhiteSpace(ItemPrice.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                items.Add(new ItemInformation
                {
                    ItemID = int.Parse(ItemID.Text),
                    ItemName = ItemName.Text,
                    ItemDescription = ItemDescription.Text,
                    ItemPrice = ItemPrice.Text
                });

                RefreshGrids();
                ClearFields();
            }
            catch
            {
                MessageBox.Show("Please enter a valid number for the ID.");
            }
        }

        private void AddToCartButtonClick(object sender, RoutedEventArgs e)
        {
            if (ItemGrid.SelectedItem is ItemInformation selected)
            {
                cartItems.Add(new ItemInformation
                {
                    ItemID = selected.ItemID,
                    ItemName = selected.ItemName,
                    ItemDescription = selected.ItemDescription,
                    ItemPrice = selected.ItemPrice
                });

                RefreshGrids();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Please select an item from the left table first.");
            }
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (ItemGrid.SelectedItem is ItemInformation selectedItem)
            {
                items.Remove(selectedItem);
                RefreshGrids();
            }
            else
            {
                MessageBox.Show("Please select an item from the available products list to remove.");
            }

            ClearFields();
        }

        private void RemoveFromCartButtonClick(object sender, RoutedEventArgs e)
        {
            if (CartGrid.SelectedItem is ItemInformation selectedCartItem)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to remove this item from the cart?\n\n" +
                    $"ID: {selectedCartItem.ItemID}\n" +
                    $"Name: {selectedCartItem.ItemName}\n" +
                    $"Description: {selectedCartItem.ItemDescription}\n" +
                    $"Price: {selectedCartItem.ItemPrice}",
                    "Confirm Remove From Cart",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    cartItems.Remove(selectedCartItem);
                    RefreshGrids();
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Please select an item from the shopping cart first.");
            }
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ItemGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemGrid.SelectedItem is ItemInformation selected)
            {
                ItemID.Text = selected.ItemID.ToString();
                ItemName.Text = selected.ItemName;
                ItemDescription.Text = selected.ItemDescription;
                ItemPrice.Text = selected.ItemPrice;
            }
        }

        private void ClearFields()
        {
            ItemID.Clear();
            ItemName.Clear();
            ItemDescription.Clear();
            ItemPrice.Clear();

            ItemGrid.SelectedItem = null;
            CartGrid.SelectedItem = null;
        }
    }
}