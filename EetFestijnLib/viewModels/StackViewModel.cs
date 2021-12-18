using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.viewModels
{
    /// <summary>
    /// A Viewmodel that has a stack of other viewmodels that can be used to show content with datatemplates.
    /// </summary>
    public class StackViewModel<T> : ViewModelBase where T : ViewModelBase
    {
        private Stack<T> ViewModels { get; }

        private T _ViewModelBase;
        /// <summary>
        /// The viewmodel for which a template is currently shown
        /// </summary>
        public T CurrentViewModel
        {
            get => _ViewModelBase;
            set => SetProperty(ref _ViewModelBase, value);
        }

        /// <summary>
        /// Creates a TemplatedStackViewModelBase
        /// </summary>
        public StackViewModel()
        {
            ViewModels = new Stack<T>();
        }

        /// <summary>
        /// Pushes a viewmodel to the stack 
        /// </summary>
        /// <param name="viewModel"></param>
        public void PushViewModel(T viewModel)
        {
            ViewModels.Push(viewModel);
            CurrentViewModel = viewModel;
        }

        /// <summary>
        /// Pops a viewmodel from the stack
        /// </summary>
        public void PopViewModel()
        {
            _ = ViewModels.Pop();
            CurrentViewModel = ViewModels.Peek();
        }

        /// <summary>
        /// Pops all viewmodels but the first
        /// </summary>
        public void PopToFirst()
        {
            while (ViewModels.Count > 1)
                _ = ViewModels.Pop();
            CurrentViewModel = ViewModels.Peek();
        }
    }
}
