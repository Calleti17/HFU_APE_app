namespace LZ1.Core.Services
{
    internal class CounterService : ICounterService
    {
        private const string ConfirmationMessageIncrement = "Are you sure you want to increment?";
        private const string ConfirmationMessageDecrement = "Are you sure you want to decrement?";

        private readonly ICounterState _state;
        private readonly IDialogService _dialogService;

        public CounterService(ICounterState state, IDialogService dialogService)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        public void Increment()
        {
            _state.Increment();
        }

        public void Decrement()
        {
            _state.Decrement();
        }

        public async Task<bool> TryIncrement()
        {
            var result = await _dialogService.AskAsync(ConfirmationMessageIncrement);

            if (result)
            {
                Increment();
            }

            return result;
        }

        public async Task<bool> TryDecrement()
        {
            var result = await _dialogService.AskAsync(ConfirmationMessageDecrement);

            if (result && _state.Count > 0)
            {
                Decrement();
                return true;
            }

            return false;
        }

        public string GetLabel()
        {
            var suffix = _state.Count == 1 ? string.Empty : "s";
            return $"Clicked {_state.Count} time{suffix}";
        }
    }
}
