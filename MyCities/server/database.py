#!/usr/bin/env python
# encoding: utf-8
"""
database.py

Created by Jonatas Chagas on 2013-11-17.
Copyright (c) 2013 __MyCompanyName__. All rights reserved.
"""

import sys
import os
from pymongo import MongoClient
from bson.json_util import dumps
import json

class Database:
	"""
	Handles all the logic regarding the database
	"""
	
	def __init__(self):
		self.conn = MongoClient('localhost', 27017)
		self.db = self.conn.mycities
	
	def update(self,collection,obj):
		name = obj['name']
		self.db[collection].update({'name' : name},obj)
		return self.retrieve(collection,name)
	
	def save(self,collection,obj):
		print obj
		name = obj['name']
		obj_db = self.retrieve(collection,name)
		if obj_db is None:
			print "inserting new object " + json.dumps(obj)
			self.db[collection].insert(obj)
		else:
			print "updating existing object " + json.dumps(obj)
			self.db[collection].update({'name' : name},obj)
		return 
	
	def retrieve(self,collection,name):
		return json.loads(dumps(self.db[collection].find_one({'name' : name})))
	
	def retrieve_user(self,user,password):
		return json.loads(dumps(self.db['users'].find_one({'user' : user,'password' : password})))