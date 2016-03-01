using System.Linq;
using Xamarin.Forms;
using FieldEngineerLite.Models;
using FieldEngineerLite.Views;
using Android.OS;
using System;

namespace FieldEngineerLite
{
    public class MyNavigationPage : NavigationPage
    {
        public MyNavigationPage(Page root)
            : base(root)
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }

    public class JobMasterDetailPage : MasterDetailPage
    {
        private JobService jobService = new JobService();
        
        public JobMasterDetailPage()
        {
            var listPage = new JobListPage(jobService);
            listPage.JobList.ItemSelected += (sender, e) =>
            {
                var selectedJob = e.SelectedItem as Job;
                if (selectedJob != null)
                {
                    NavigateTo(e.SelectedItem as Job);
                }
            };

            var listNavigationPage = new MyNavigationPage(listPage);
            listNavigationPage.Title = "Appointments";
            Master = listNavigationPage;
            var details = new JobDetailsPage(jobService);

            details.Content.IsVisible = false;
            Detail = new MyNavigationPage(details);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await jobService.InitializeAsync();
			await jobService.SyncAsync();

			var jobs = await jobService.ReadJobs("");

			if (jobs.Any(x => x.Version != null || x.UpdatedAt != null || x.CreatedAt != null))
			{
				Console.WriteLine($"Version, UpdatedAt, CreatedAt are not all null");
			}
			else
			{
				Console.WriteLine($"Version, UpdatedAt, CreatedAt  all null");
			}


			if (jobs.Count() > 0)
            {
                Job job = jobs.First();
                NavigateTo(job);
            }
        }

        public void NavigateTo(Job item)
        {
            var page = new JobDetailsPage(jobService);
            page.BindingContext = item;
            Detail = new NavigationPage(page);
            IsPresented = false;
        }
    }
}

