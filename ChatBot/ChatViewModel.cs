using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatBot
{
    public partial class ChatViewModel : ObservableObject
    {
        private readonly ForefrontAIService _forefrontAIService;

        [ObservableProperty]
        private string userMessage;

        [ObservableProperty]
        private string aiResponse;

        [ObservableProperty]
        private string tokenUsage; // Displays token usage info

        public ObservableCollection<string> ChatHistory { get; set; }

        public ChatViewModel()
        {
            _forefrontAIService = new ForefrontAIService();
            ChatHistory = new ObservableCollection<string>();
        }

        [RelayCommand]
        public async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(UserMessage)) return;

            ChatHistory.Add($"You: {UserMessage}");
            var (response, usage) = await _forefrontAIService.GetAIResponseAsync(UserMessage);
            ChatHistory.Add($"AI: {response}");

            AiResponse = response;
            TokenUsage = $"Tokens Used: {usage.TotalTokens} (Input: {usage.InputTokens}, Output: {usage.OutputTokens})";

            UserMessage = string.Empty;
        }
    }
}
