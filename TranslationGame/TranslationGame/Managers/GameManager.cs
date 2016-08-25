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
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using AppServiceHelpers.Abstractions;
using Akavache;

namespace TranslationGame
{
	public static class GameManager
	{
		private const string WORDS_KEY = "WORDS_KEY/{0}";

		public static string Language { get; set; }

		private static List<Word> words;

		public static async Task GetQuestions()
		{
			var localCache = ((App)Xamarin.Forms.Application.Current).LocalCache;
			words = localCache.GetOrCreateObject(String.Format(WORDS_KEY, Language), () => new List<Word>()).Wait();

			await AddWords();

			var table = App.Client.Table<Word>();
			var results = await table.GetItemsAsync();
			foreach (var word in results)
			{
				if (words.All(w => w.Text != word.Text))
				{
					words.Add(word);
				}
			}
		}

		public static async Task<bool> AddNewWord(string word)
		{
			var table = App.Client.Table<Word>();
			bool created = await table.AddAsync(new Word()
				{
					Text = word
				});
			return created;
		}

		private static async Task SetTranslatedText(Word word)
		{
			if (String.IsNullOrEmpty(word.TranslatedText))
			{
				TranslateService service = new TranslateService();
				word.TranslatedText = await service.TranslateString(word.Text, Language);
			}
		}

		public static async Task<QuestionDTO> GetNewQuestion()
		{
			Random rand = new Random();
			int rightAnswer = rand.Next(1, 4);
			int currentWord = rand.Next(0, words.Count);
			int option1 = rand.Next(0, words.Count);

			while (option1 == currentWord)
			{
				option1 = rand.Next(0, words.Count);
			}

			int option2 = rand.Next(0, words.Count);

			while (option2 == currentWord || option2 == option1)
			{
				option2 = rand.Next(0, words.Count);
			}

			QuestionDTO question = new QuestionDTO();
			question.CurrentWord = words[currentWord].Text;
			question.RightAnswer = rightAnswer;

			await SetTranslatedText(words[option1]);
			await SetTranslatedText(words[option2]);
			await SetTranslatedText(words[currentWord]);

			var localCache = ((App)Xamarin.Forms.Application.Current).LocalCache;
			localCache.InsertObject(String.Format(WORDS_KEY, Language), words).Wait();

			question.Word1 = rightAnswer == 1 ? 
				words[currentWord].GetTranslatedText() : words[option1].GetTranslatedText();
			
			question.Word2 = rightAnswer == 2 ? 
				words[currentWord].GetTranslatedText() : 
				rightAnswer < 2 ? words[option1].GetTranslatedText() : words[option2].GetTranslatedText();
			
			question.Word3 = rightAnswer == 3 ? 
				words[currentWord].GetTranslatedText() : words[option2].GetTranslatedText();

			return question;
		}

		private static async Task AddWords()
		{
			var table = App.Client.Table<Word>();

			List<string> texts = new List<string>();
			texts.Add("Fork");
			texts.Add("Knife");
			texts.Add("Spoon");
			texts.Add("Word");
			texts.Add("World");
			texts.Add("Text");
			texts.Add("Ball");
			texts.Add("Bear");
			texts.Add("Paper");
			texts.Add("Computer");
			texts.Add("Mobile");
			texts.Add("Card");
			texts.Add("Painting");
			texts.Add("Car");
			texts.Add("Skirt");
			texts.Add("Watermelon");
			texts.Add("Melon");
			texts.Add("Strawberry");
			texts.Add("Pearl");
			texts.Add("Plum");
			texts.Add("Photo");
			texts.Add("Truck");
			texts.Add("Bike");
			texts.Add("Motorcycle");
			texts.Add("Father");
			texts.Add("Mother");
			texts.Add("Sun");
			texts.Add("Grandfather");
			texts.Add("Cousin");
			texts.Add("Law");
			texts.Add("Technology");
			texts.Add("Nurse");
			texts.Add("Doctor");
			texts.Add("Police");
			texts.Add("Fire");
			texts.Add("Water");
			texts.Add("Wind");
			texts.Add("Thunder");
			texts.Add("Lake");
			texts.Add("Wolf");
			texts.Add("Heat");
			texts.Add("Cold");
			texts.Add("Bull");
			texts.Add("Hawk");
			texts.Add("Black");
			texts.Add("White");
			texts.Add("Purple");
			texts.Add("Green");
			texts.Add("Yellow");
			texts.Add("Rocket");
			texts.Add("Patriot");
			texts.Add("Track");
			texts.Add("Soccer");
			texts.Add("Field");

			var results = await table.GetItemsAsync();
			foreach (var word in results)
			{
				if (words.All(w => w.Text != word.Text))
				{
					words.Add(word);
				}
			}

			bool created;
			foreach (var text in texts)
			{
				if (words.All(w => w.Text != text))
				{
					Word word = new Word()
					{
						Text = text
					};

					created = await table.AddAsync(word);
					words.Add(word);
				}
			}
		}
	}
}

