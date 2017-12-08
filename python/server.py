"""

@author GalenS <galen.scovelL@gmail.com>
"""

from flask import Flask, jsonify, request
from uuid import uuid4

from chain import Chain

import util as util


app = Flask(__name__)
node_identifier = str(uuid4()).replace('-', '')
local_chain = Chain()


@app.route('/mine', methods=['GET'])
def mine():
    last_block = local_chain.last_block
    last_proof = last_block.proof
    proof = util.proof_of_work(last_proof)

    # Receive reward for finding proof, sender is 0 to show that this node has mined a new coin
    local_chain.new_transaction(
        sender='0',
        recipient=node_identifier,
        amount=1
    )

    # Forge new block by adding it to the chain
    previous_hash = util.hash_block(last_block)
    block = local_chain.new_block(proof, previous_hash)

    response = {
        'message': "New Block Forged",
        'index': block.index,
        'transactions': block.transactions,
        'proof': block.proof,
        'previous_hash': block.previous_hash,
    }

    return jsonify(response), 200


@app.route('/nodes/register', methods=['POST'])
def register_nodes():
    values = request.get_json()

    nodes = values.get('nodes')
    if not nodes:
        return "Error: Please supply a valid list of nodes", 400

    for node in nodes:
        local_chain.register_node(node)

    response = {
        'message': 'New nodes have been added',
        'total_nodes': list(local_chain.nodes),
    }

    return jsonify(response), 201


@app.route('/nodes/resolve', methods=['GET'])
def consensus():
    replaced = util.resolve_conflicts(local_chain)

    if replaced:
        response = {
            'message': 'Our chain was replaced',
            'new_chain': local_chain.chain
        }
    else:
        response = {
            'message': 'Our chain is authoritative',
            'chain': local_chain.chain
        }

    return jsonify(response), 200


@app.route('/transactions/new', methods=['POST'])
def new_transaction():
    values = request.get_json()

    required = ['sender', 'recipient', 'amount']
    if not all(k in values for k in required):
        return 'Missing required values', 400

    index = local_chain.new_transaction(
        values['sender'], values['recipient'], values['amount']
    )

    response = {
        'message': f'Transaction added to block at {index}'
    }

    return jsonify(response), 201


@app.route('/chain', methods=['GET'])
def full_chain():
    response = {
        'chain': local_chain.chain,
        'length': len(local_chain.chain)
    }

    return jsonify(response), 200


if __name__ == '__main__':
    app.run(
        host='0.0.0.0',
        port=5000
    )
