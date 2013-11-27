#!/usr/bin/env python
# encoding: utf-8
"""
example_server.py

Created by Jonatas Chagas on 2013-11-23.
Copyright (c) 2013 __MyCompanyName__. All rights reserved.
"""

import cherrypy

class HelloWorld(object):
    def _get_jpeg_data(self):
        """This method should return the jpeg data"""
        return ""

    @cherrypy.expose
    def index(self):
        cherrypy.response.headers['Content-Type'] = 'application/jpeg'
        return self._get_jpeg_data()

cherrypy.config.update({'server.socket_host': '64.72.221.48',
	'server.socket_port': 8080,})
	
cherrypy.quickstart(HelloWorld())