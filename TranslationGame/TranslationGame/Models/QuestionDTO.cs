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

namespace TranslationGame
{
	public class QuestionDTO
	{
		public string CurrentWord { get; set; }

		public string Word1 { get; set; }
		public string Word2 { get; set; }
		public string Word3 { get; set; }

		public int RightAnswer { get; set; }
	}
}

