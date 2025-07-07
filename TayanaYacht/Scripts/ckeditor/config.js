/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	config.removeDialogTabs = 'image:Upload;image:advanced;image:Link';

	// (關鍵) 關閉對沒有區塊元素包裹的內容自動加上 <p>
	config.autoParagraph = false;

};
