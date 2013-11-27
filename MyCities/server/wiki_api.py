#!/usr/bin/env python
# encoding: utf-8
"""
wiki_api.py

Scrapes main paragraph of information from wikipedia

Created by Jonatas Chagas on 2013-11-17.
Copyright (c) 2013 __MyCompanyName__. All rights reserved.
"""

import sys
import os
from bs4 import BeautifulSoup
import urllib
from urllib2 import urlopen


def get_description(query):
	"""
	Scrapes main paragraph of information from wikipedia
	"""
	url = "http://en.wikipedia.org/wiki/" + query
	page = urllib.urlopen(url)
	html_content = page.read()
	page.close()
 	soup = BeautifulSoup(html_content)
	text = ''
	for info in soup.find(id="mw-content-text").find_all('p'):
		text +=  info.get_text()
		if len(text) == 0:
			break
	return text