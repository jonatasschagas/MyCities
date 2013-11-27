#!/usr/bin/env python
# encoding: utf-8
"""
n_server.py

Created by Jonatas Chagas on 2013-11-23.
Copyright (c) 2013 __MyCompanyName__. All rights reserved.
"""

import sys
import os
from bing_api import *
from wiki_api import *
from yahoo_api import *
from database import *
import json
import cherrypy


class MobileBackend(object):
	def index(self,action,city_name):
		if action == 'all_info':
			city = database.retrieve('cities',city_name)
			
			if not city:
				print city_name + " not found in the database. Loading info from the web"
				city = {}
				pictures = get_pictures(city_name)
				desc = get_description(city_name)
				city["pictures"] = pictures
				city["description"] = desc
				city["name"] = city_name
				database.save('cities',city)
				
			## city weather cannot be loaded from the
			## database. The weather changes all the time :)
			weather = get_city_weather(city_name)
			city["weather"] = weather
			return json.dumps({"name" : city['name'],"weather" : weather, "pictures" : city['pictures'], "description" : city['description']})
		return "Nothing"
	index.exposed = True		

	@cherrypy.expose
	def save_user(self,user_json):
		user_json = json.loads(user_json)
		database.save('users',user_json)
		return "OK!"
		
	@cherrypy.expose
	def login(self,user,password):
		user = database.retrieve_user(user,password)
		if not user is None:
			return json.dumps({"name" : user["name"], "password" : user["password"], "login_state" : "ok", "fb_id" : user["fb_id"],"first_name" : user["first_name"],"last_name" : user["last_name"]})
		else:
			return json.dumps({"login_state" : "error"})
	
	@cherrypy.expose
	def signin(self,user,password):
		database.save('users',{"name" : user, "password" : password, "login_state" : "ok", "fb_id" : "","first_name" : "","last_name" : ""})
		return json.dumps({"name" : user, "password" : password, "login_state" : "ok", "fb_id" : "","first_name" : "","last_name" : ""})
	
	@cherrypy.expose
	@cherrypy.tools.json_out()
	def load_user(self,name):
		user = database.retrieve('users',name)
		return json.dumps(user)
	
	@cherrypy.expose
	def save_user_cities(self,name,city):
		user_cities = database.retrieve('user_cities',name)
		if user_cities is None:
			user_cities = {"name" : name, "cities" : [city]}
			database.save('user_cities',user_cities)
		elif city not in user_cities["cities"]:
			user_cities["cities"].append(city)
			user_cities = {"name" : name, "cities" : user_cities["cities"]}
			database.save('user_cities',user_cities)
		return "OK!"
	
	@cherrypy.expose
	def load_user_cities(self,name):
		user_cities = database.retrieve('user_cities',name)
		cities = []
		for city_name in user_cities["cities"]:
			cities.append({"name" : city_name})
		return json.dumps({ "name" : user_cities["name"] , "cities" : cities })
		
	@cherrypy.expose
	def get_city_by_geoloc(self,lat,lon):
		return json.dumps({"name" : get_city_by_geoloc(lat,lon)})
	
database = Database()
def main():
	cherrypy.config.update({'server.socket_host': '0.0.0.0','server.socket_port': 8080,})
	cherrypy.quickstart(MobileBackend())

if __name__ == '__main__':
	main()

