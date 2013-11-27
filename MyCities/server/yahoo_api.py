#!/usr/bin/env python
# encoding: utf-8
"""
yahoo_api.py

Provides geo-location information, weather and forecast

Created by Jonatas Chagas on 2013-11-17.
Copyright (c) 2013 __MyCompanyName__. All rights reserved.
"""

import sys
import os
import requests
import yql
import json

def get_woeid(city):
	y = yql.Public()
	query = "select woeid,centroid.latitude,centroid.longitude from geo.places where text=@text"
	result = y.execute(query, {"text": city})
	return result.rows[0]['woeid']

def get_lot_lon(city):
	y = yql.Public()
	query = "select woeid,centroid.latitude,centroid.longitude from geo.places where text=@text"
	result = y.execute(query, {"text": city})
	print json.dumps(result.rows[0])
	return {'lat' : result.rows[0]['centroid']['latitude'], 'lon' : result.rows[0]['centroid']['longitude']}

def get_weather(woeid):
	y = yql.Public()
	query = "select item.condition.code,item.condition.temp,item.condition.text from weather.forecast where woeid=@woeid and u='c'"
	result = y.execute(query, {"woeid": woeid})
	return result.rows

def get_forecast(woeid):
	y = yql.Public()
	query = "select item.forecast from weather.forecast where woeid=@woeid and u='c'"
	result = y.execute(query, {"woeid": woeid})
	forecast = []
	for row in result.rows:
		forecast.append(row['item']['forecast'])
	return forecast

def get_city_weather(city):
	woeid = get_woeid(city)
	location = get_lot_lon(city)
	weather = get_weather(woeid)
	forecast = get_forecast(woeid)
	city_info = {'woeid' : woeid, 'weather' : weather, 'forecast' : forecast, 'lat' : location['lat'], 'lon' : location['lon']}
	return city_info

def get_city_by_geoloc(lat,lon):
	y = yql.Public()
	query = 'select * from geo.placefinder where text="'+lat + ',' + lon+'" and gflags="R"'
	result = y.execute(query)
	for row in result.rows:
		return row['city']
	return None