using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerCourse.Client.Views.AddWindows;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services
{
    public class CommonViewService
    {
        private string imageDialogFilterPattern = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        public Window CurrentOpenWindow { get; private set; }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowActionResult(HttpStatusCode code, string message) 
        {
            if (code == HttpStatusCode.OK)
            {
                ShowMessage(code.ToString() + $"\n{message}");
            } else
            {
                ShowMessage(code.ToString() + $"\nError!");
            }
        }
        public void OpenWindow(Window window, BindableBase viewModel)
        {
            CurrentOpenWindow = window;
            window.DataContext = viewModel;
            window.ShowDialog();
        }
        public string GetFileFromDialog(string filter)
        {
            string filePath = string.Empty;
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = filter;

            bool? result = dialog.ShowDialog();
            if(result == true)
            {
                filePath = dialog.FileName;
            }
            return filePath;
        }
        
        public void SetPhotoForObject(CommonModel model)
        {
            string photoPath = GetFileFromDialog(imageDialogFilterPattern);
            if (string.IsNullOrWhiteSpace(photoPath) == false)
            {
                var photoBytes = File.ReadAllBytes(photoPath);
                model.Photo = photoBytes;
            }
        }
    }
}
