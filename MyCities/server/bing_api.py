#!/usr/bin/env python
# encoding: utf-8
"""
bing_api.py

Returns the pictures from Bing API

Created by Jonatas Chagas on 2013-11-17.
Copyright (c) 2013 __MyCompanyName__. All rights reserved.
"""

import sys
import os
from pybing import Bing
import base64
import requests
from StringIO import StringIO
from PIL import Image
from urlparse import urlparse
from os.path import splitext, basename
from requests.auth import HTTPBasicAuth

USER="jonataschagas@gmail.com"
ACCOUNT_KEY = "JyCxcR0J6M5bnXslYYo32hzQh1aBuSfrIcc+bhtQ1/s"
API_URL = "https://api.datamarket.azure.com/Bing/Search"
IMAGES_FOLDER="/var/www/html/"
IMAGES_URL="http://54.204.10.235/"

def saveImage(url,image_name):
	try:
		r = requests.get(url)
		disassembled = urlparse(url)
		filename, extension = splitext(basename(disassembled.path))
		i = Image.open(StringIO(r.content))
		image_path=image_name +  extension
		image_path_thumb=image_name + "_thumb" + extension
		i.save(IMAGES_FOLDER + image_path)
		## resize image for thumbnail
		size = 128, 128
		try:
			im = Image.open(IMAGES_FOLDER + image_path)
			im.thumbnail(size, Image.ANTIALIAS)
			im.save(IMAGES_FOLDER + image_path_thumb)
		except IOError as e:
			print "cannot create thumbnail for " + image_path_thumb
		return {"image" : IMAGES_URL + image_path , "image_thumb" : IMAGES_URL + image_path_thumb}
	except Exception as e:
		print "cannot create picture for " + image_name
	return None

def get_pictures(query):
	"""
	Returns the pictures from Bing API
	"""
	serviceOp = "Image"
	url = API_URL + "/" + serviceOp + "?$format=json&Query='" + query + "'"
	r = requests.get(url,auth=HTTPBasicAuth(USER,ACCOUNT_KEY))
	pictures = []
	i = 0
	for pics in r.json()['d']['results']:
		localPic = saveImage(pics['MediaUrl'],query + "_" +str(i))
		if not localPic is None:
			pictures.append(localPic)
			i+=1

		## maximum 15 pictures
		if i == 15:
			break

	return pictures

