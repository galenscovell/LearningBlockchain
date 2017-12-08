"""

@author GalenS <galen.scovelL@gmail.com>
"""


class Transaction(dict):
    def __init__(self, sender, recipient, amount):
        self.sender = sender
        self.recipient = recipient
        self.amount = amount

        dict.__init__(
            self,
            sender=sender,
            recipient=recipient,
            amount=amount
        )
