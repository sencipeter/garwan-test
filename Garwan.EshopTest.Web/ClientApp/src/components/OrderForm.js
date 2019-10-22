import React, { Component } from 'react';

export class OrderForm extends Component {
    constructor(props) {
        super(props);

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.state = {
            message: '',
            price: props.price,
            order:
            {
                test: '',
                productId: props.id,
                productCount: 1
            }
        };
    }

    handleChange(event) {
        let newState = Object.assign({}, this.state);
        newState.order.productCount = event.target.value;
        this.setState(newState);
    }

    handleSubmit(event) {
        event.preventDefault();
        var order = this.state.order;
        order.time = new Date();
        order.totalPrice = this.state.price * order.productCount;
        fetch('api/order/post', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(order)
        }).then(response => response.json())
            .then(data => {
                let newState = Object.assign({}, this.state);
                if (data.status) {
                    var message = data.title;
                    if (data.errors) {
                        message = <ul>
                            {Object.keys(data.errors).map((error, index) => (
                                <li>{data.errors[error][0]}</li>
                            ))}
                        </ul>
                    }
                    else
                        message = data.title;
                    newState.message = <div className="alert alert-danger">{message}</div>
                }
                else {
                    newState.message = <div className="alert alert-success">Thanks for your order {data.result.id}</div>
                }
                this.setState(newState);
            });
    }

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <div>
                    Summary: {this.state.order.productCount} x {this.state.price} = {this.state.price * this.state.order.productCount} &euro;
                </div>
                <label>
                    Qty:<input value={this.state.order.productCount} onChange={this.handleChange} required type="number" min="1" />
                </label>
                <input type="submit" value="Order" />
                <div>
                    {this.state.message}
                </div>
            </form>

        );
    }
}