"""
The core blockchain.

@author GalenS <galen.scovelL@gmail.com>
"""

from time import time
from urllib.parse import urlparse

from block import Block
from node import Node
from transaction import Transaction


class Blockchain(object):
    def __init__(self):
        self.chain = []
        self.current_transactions = []
        self.nodes = []

        # Create the genesis block with no predecessors
        self.new_block(proof=100, previous_hash=1)

    def register_node(self, address):
        parsed_url = urlparse(address)
        node = Node(parsed_url.netloc)
        self.nodes.append(node)

    def new_block(self, proof, previous_hash=None):
        """
        Creates a new Block and adds it to the chain
        """
        block = Block(
            index=len(self.chain) + 1,
            timestamp=time(),
            transactions=self.current_transactions,
            proof=proof,
            previous_hash=previous_hash
        )

        # Reset current list of transactions
        self.current_transactions = []

        self.chain.append(block)

        return block

    def new_transaction(self, sender, recipient, amount):
        """
        Adds a new transaction to the list of transactions
        """
        transaction = Transaction(sender, recipient, amount)
        self.current_transactions.append(transaction)

        return self.last_block.index + 1

    @property
    def last_block(self):
        """
        Returns the last Block in the chain
        """
        return self.chain[-1]
