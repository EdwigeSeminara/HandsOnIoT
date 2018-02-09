using HandsOnIoT.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HandsOnIoT
{
    /// <summary>
    /// Base class for testable GrovePi sensors.
    /// </summary>
    internal class SensorBase : ViewModelBase
    {
        private string name;
        private string port;
        private string state;
        private bool isUnterTest;
        private string testDescription;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Port
        {
            get { return port; }
            set { SetProperty(ref port, value); }
        }

        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        public bool IsUnderTest
        {
            get { return isUnterTest; }
            set { SetProperty(ref isUnterTest, value); }
        }

        public string ImagePath { get; set; }

        public string TestDescription
        {
            get { return testDescription; }
            set { SetProperty(ref testDescription, value); }
        }

#pragma warning disable CS1998
        // Async method lacks 'await' operators and will run synchronously
        public virtual async Task Test()
        { }

        public virtual async Task Send() { }
#pragma warning restore CS1998

        public ICommand TestCommand
        {
            get { return new DelegateCommand(Test_Executed, Test_CanExecute); }
        }

        public ICommand SendCommand
        {
            get { return new DelegateCommand(SendExecuted); }
        }

        private async void SendExecuted()
        {
            await RunTheSend();
        }

        private bool Test_CanExecute()
        {
            return !IsUnderTest;
        }

        private async void Test_Executed()
        {
            await RunTheTest();
        }

        private async Task RunTheTest()
        {
            IsUnderTest = true;
            try
            {
                await Test();
            }
            catch (Exception ex)
            {
                State = ex.Message;
                Debug.WriteLine("Error : " + this.Name + " - " + ex.Message);
            }

            IsUnderTest = false;
        }

        private async Task RunTheSend()
        {
            IsUnderTest = true;
            try
            {
                await Send();
            }
            catch (Exception ex)
            {
                State = ex.Message;
                Debug.WriteLine("Error : " + this.Name + " - " + ex.Message);
            }

            IsUnderTest = false;
        }
    }

}
