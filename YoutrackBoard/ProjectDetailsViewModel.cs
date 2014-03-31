using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using ReactiveUI;


namespace YoutrackBoard
{
    using YoutrackBoard.ReviewboardApi;

    class ProjectDetailsViewModel: Screen
    {
        private Project _project;
        private readonly Sprint _sprint;
        private readonly IssueRepository _issueRepository;
        private readonly ReviewRepository _reviewRepository;
        private readonly ProjectRepository _projectRepository;

        public ProjectDetailsViewModel(
            Project project, 
            Sprint sprint, 
            IssueRepository issueRepository,
            ReviewRepository reviewRepository,
            Func<Person, PersonDetailsViewModel> personDetailsFactory
            )
        {
            _project = project;
            _sprint = sprint;
            _issueRepository = issueRepository;
            this._reviewRepository = reviewRepository;

            Name = _project.Name;
            PersonDetails = new ObservableCollection<PersonDetailsViewModel>(
                _project.People.Select(personDetailsFactory)
                );
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            LoadData();
        }

        private async void LoadData()
        {
            await Task.Delay(500);
            Dictionary<string, PersonDetailsViewModel> viewModels = new Dictionary<string, PersonDetailsViewModel>();

            var result = await _issueRepository.Search(_project, _sprint);
            foreach (var personDetailsViewModel in PersonDetails)
            {
                viewModels.Add(personDetailsViewModel.Person.Login, personDetailsViewModel);
                personDetailsViewModel.SetIssues(
                    result.Where(issue => issue.Assignee!= null 
                        && issue.Assignee.Login == personDetailsViewModel.Person.Login).ToArray());
            }
            //load for all isues
            var tasks = result.Select(issue => _issueRepository.GetWorkItems(issue)).ToArray();
            var results = await Task.WhenAll(tasks);

            foreach (var group in results.SelectMany(r=>r).GroupBy(r=>r.Author.Login))
            {
                var personViewModel = viewModels[group.Key];
                    personViewModel.SetWorkItems(group.ToArray());
            }

            var reviews = await _reviewRepository.GetReviews();
            foreach (var reviewRequest in reviews)
            {
                
            }



        }

        public ObservableCollection<PersonDetailsViewModel> PersonDetails { get; set; }

        public string Name { get; set; }
    }
}