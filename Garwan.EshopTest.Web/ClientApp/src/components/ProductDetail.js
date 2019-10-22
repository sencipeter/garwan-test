import React, { Component } from 'react';
import { OrderForm } from '../components/OrderForm';

export class ProductDetail extends Component {
    static displayName = ProductDetail.name;

    constructor(props) {
        super(props);
        this.state = {
            product: null, loading: true, error: '',
            hasError: false
        };
        var id = this.props.match.params["id"];

        fetch('api/product/detail/' + id)
            .then(response => response.json())
            .then(data => {
                if (data.status) {
                    this.setState({
                        product: null, loading: false, error: data.title,
                        hasError: true
                    });
                }
                else {
                    this.setState({
                        product: data.result, loading: false, error: '',
                        hasError: false
                    });
                }
            });
    }

    static renderProductDetail(product) {
        return (
            <div>
                <h1>{product.name}</h1>
                <p>{product.description}</p>
                <h2>Category</h2>
                <p>{product.animalCategory.name}</p>
                <h2>Price</h2>
                <p>{product.price} &euro;</p>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            :
            <div>
                {
                    this.state.hasError ?
                        <div className="row">
                            <div className='col-sm-12'>
                                <div className='alert alert-danger'>{this.state.error}</div>
                            </div>
                        </div> :
                        <div className="row">
                            <div className="col-sm-6">
                                {ProductDetail.renderProductDetail(this.state.product)}
                            </div>
                            <div className="col-sm-6">
                                <OrderForm {...this.state.product} />
                            </div>
                        </div>
                }
            </div>

        return (
            <div>
                {contents}
            </div>
        );
    }
}
