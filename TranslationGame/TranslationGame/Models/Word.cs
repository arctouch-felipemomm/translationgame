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
using System.Runtime.Serialization;

using AppServiceHelpers.Models;

namespace TranslationGame
{
	[DataContract(Name = "word")]
	public class Word : EntityData
	{
		[DataMember]
		public string Text { get; set; }

		[DataMember]
		public string TranslatedText { get; set; }

		public string GetTranslatedText()
		{
			return String.IsNullOrEmpty(TranslatedText) ? Text : TranslatedText;
		}
	}
}