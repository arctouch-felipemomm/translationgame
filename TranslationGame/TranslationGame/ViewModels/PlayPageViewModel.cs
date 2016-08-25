//
// Copyright 2016 ArcTouch LLC.
// All rights reserved.
//
// This file, its contents, concepts, methods, behavior, and operation
// (collectively the "Software") are protected by trade secret, patent,
// and copyright laws. The use of the Software is governed by a license
// agreement. Disclosure of the Software to third parties, in any form,
// in whole or in part, is expressly prohibited except as authorized by
// the license agreement.
//

using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace TranslationGame
{
	public class PlayPageViewModel : ViewModelBase
	{
		private bool isOnHold;
		public bool IsOnHold
		{
			get
			{
				return this.isOnHold;
			}

			set
			{
				if (this.isOnHold != value)
				{
					this.isOnHold = value;
					RaisePropertyChanged();
				}
			}
		}

		private bool isPlaying;
		public bool IsPlaying
		{
			get
			{
				return this.isPlaying;
			}

			set
			{
				if (this.isPlaying != value)
				{
					this.isPlaying = value;
					RaisePropertyChanged();
				}
			}
		}

		private string language;
		public string Language
		{
			get
			{
				return this.language;
			}

			set
			{
				if (this.language != value)
				{
					this.language = value;
					RaisePropertyChanged();
				}
			}
		}

		private string currentWord;
		public string CurrentWord
		{
			get
			{
				return this.currentWord;
			}

			set
			{
				if (this.currentWord != value)
				{
					this.currentWord = value;
					RaisePropertyChanged();
				}
			}
		}

		private string word1;
		public string Word1
		{
			get
			{
				return this.word1;
			}

			set
			{
				if (this.word1 != value)
				{
					this.word1 = value;
					RaisePropertyChanged();
				}
			}
		}

		private string word2;
		public string Word2
		{
			get
			{
				return this.word2;
			}

			set
			{
				if (this.word2 != value)
				{
					this.word2 = value;
					RaisePropertyChanged();
				}
			}
		}

		private string word3;
		public string Word3
		{
			get
			{
				return this.word3;
			}

			set
			{
				if (this.word3 != value)
				{
					this.word3 = value;
					RaisePropertyChanged();
				}
			}
		}

		private string scoreText;
		public string ScoreText
		{
			get
			{
				return this.scoreText;
			}

			set
			{
				if (this.scoreText != value)
				{
					this.scoreText = value;
					RaisePropertyChanged();
				}
			}
		}

		private string word;
		public string Word
		{
			get
			{
				return this.word;
			}

			set
			{
				if (this.word != value)
				{
					this.word = value;
					RaisePropertyChanged();
				}
			}
		}

		private bool isLoading;
		public bool IsLoading
		{
			get
			{
				return this.isLoading;
			}

			set
			{
				if (this.isLoading != value)
				{
					this.isLoading = value;
					RaisePropertyChanged();
				}
			}
		}

		public ICommand PlayCommand { get; private set; }

		public ICommand StopCommand { get; private set; }

		public ICommand AddWordCommand { get; private set; }

		public ICommand AnswerWord1Command { get; private set; }

		public ICommand AnswerWord2Command { get; private set; }

		public ICommand AnswerWord3Command { get; private set; }

		private int points;
		private int answers;
		private int rightAnswer;

		public PlayPageViewModel()
		{
			IsPlaying = false;
			IsOnHold = true;

			PlayCommand = new Command(Play);
			StopCommand = new Command(Stop);
			AddWordCommand = new Command(AddWord);
			AnswerWord1Command = new Command(() => Answer(1));
			AnswerWord2Command = new Command(() => Answer(2));
			AnswerWord3Command = new Command(() => Answer(3));
		}

		private async void Play()
		{
			if (!IsPlaying)
			{
				if (!IsValidLanguage())
				{
					return;
				}

				IsLoading = true;
				IsPlaying = true;
				IsOnHold = false;

				CurrentWord = "";
				Word1 = "";
				Word2 = "";
				Word3 = "";
				points = 0;
				answers = 0;

				SetScoreText();

				GameManager.Language = Language.ToUpperInvariant();
				await GameManager.GetQuestions();
				await GetNewQuestion();
			}
		}

		private void Stop()
		{
			if (IsPlaying)
			{
				IsPlaying = false;
				IsOnHold = true;
			}
		}

		private async void AddWord()
		{
			if (!String.IsNullOrEmpty(Word))
			{
				await GameManager.AddNewWord(Word);

				Word = "";
			}
		}

		private async void Answer(int answer)
		{
			if (answer.Equals(rightAnswer))
			{
				points++;
			}

			answers++;

			SetScoreText();

			IsLoading = true;
			await GetNewQuestion();
		}

		private async Task GetNewQuestion()
		{
			QuestionDTO question = await GameManager.GetNewQuestion();

			CurrentWord = question.CurrentWord;
			Word1 = question.Word1;
			Word2 = question.Word2;
			Word3 = question.Word3;
			rightAnswer = question.RightAnswer;

			IsLoading = false;
		}

		private void SetScoreText()
		{
			ScoreText = $"Score: {points} / {answers}";
		}

		private bool IsValidLanguage()
		{
			if (!String.IsNullOrEmpty(Language)
			    && (Language.ToLowerInvariant().Equals("pt")
			    || Language.ToLowerInvariant().Equals("de")
			    || Language.ToLowerInvariant().Equals("it")
			    || Language.ToLowerInvariant().Equals("es")
			    || Language.ToLowerInvariant().Equals("fr")))
			{
				return true;
			}
			else
			{
				Application.Current.MainPage.DisplayAlert("Alert", "Valid Languages: pt, de, it, es and fr!", "OK");
				return false;
			}
		}
	}
}