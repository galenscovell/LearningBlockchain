"""

@author GalenS <galen.scovelL@gmail.com>
"""


class Node(dict):
    def __init__(self, url):
        self.url = url

        dict.__init__(self, url=url)
