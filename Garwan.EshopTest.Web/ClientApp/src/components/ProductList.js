import React, { Component } from 'react';
import { Link } from 'react-router-dom';
export class ProductList extends Component {
    static displayName = ProductList.name;

    constructor(props) {
        super(props);
        this.state = {
            products: [],
            hasError: false,
            error: '',
            page: 1,
            pageSize: 10,
            totalPage: 0,
            loading: true
        };
        var page = 1;
        var pageSize = 10;
        if (this.props.match.params["page"])
            page = this.props.match.params["page"];
        if (this.props.match.params["pageSize"])
            pageSize = this.props.match.params["pageSize"];
        fetch('api/product/list?page=' + page + '&pageSize=' + pageSize)
            .then(response => response.json())
            .then(data => {
                if (data.status) {
                    this.setState(
                        {
                            products: [],
                            loading: false,
                            hasError: true,
                            error: data.title,
                        });
                }
                else {
                    this.setState(
                        {
                            hasError: false,
                            error: '',
                            products: data.result,
                            page: data.currentPage,
                            pageSize: data.pageSize,
                            totalPage: data.totalPage,
                            loading: false
                        });
                }
            });
    }

    componentDidUpdate(prevProps) {
        if (prevProps.location.key !== this.props.location.key) {
            this.setState({
                loading: true
            });
            var page = 1;
            if (this.props.location.state && this.props.location.state.page)
                page = this.props.location.state.page;
            var pageSize = 10;
            if (this.state.pageSize)
                pageSize = this.state.pageSize;
            fetch('api/product/list?page=' + page + '&pageSize=' + pageSize)
                .then(response => response.json())
                .then(data => {
                    if (data.status) {
                        this.setState(
                            {
                                products: [],
                                loading: false,
                                hasError: true,
                                error: data.title,
                            });
                    }
                    else {
                        this.setState(
                            {
                                products: data.result,
                                page: data.currentPage,
                                pageSize: data.pageSize,
                                totalPage: data.totalPage,
                                loading: false,
                                error: '',
                                hasError: false
                            });
                    }
                });
        }
    }

    static renderProductTable(products) {
        return (
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>Product name</th>
                        <th>Price (&euro;)</th>

                    </tr>
                </thead>
                <tbody>
                    {products.map(product =>
                        <tr key={product.id}>
                            <td>
                                <Link to={{
                                    pathname: '/product/detail/' + product.id
                                }}>
                                    {product.name}
                                </Link>
                            </td>
                            <td>{product.price}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    static renderPager(page, totalPage) {
        return (
            <ul className="pagination">
                <li className="page-item">
                    {
                        page > 1 ?
                            page == 1
                                ?
                                <Link to={{
                                    state: { page: 1 },
                                    pathname: '/products'
                                }} className="page-link active">
                                    Previous
                            </Link>
                                :
                                <Link to={{
                                    state: { page: page - 1 },
                                    pathname: '/products/' + (page - 1)
                                }} className="page-link active">
                                    Previous
                            </Link> :
                            <a className="page-link disabled">Previous</a>
                    }
                </li>

                <li className="page-item">
                    {
                        page < totalPage ?
                            <Link to={{
                                state: { page: page + 1 },
                                pathname: '/products/' + (page + 1),
                            }} className="page-link active">
                                Next
                            </Link> :
                            <a className="page-link disabled">Next</a>
                    }

                </li>
            </ul>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            :
            <div>
                {!this.state.hasError ?
                    <div>
                        {ProductList.renderProductTable(this.state.products)}
                        {ProductList.renderPager(this.state.page, this.state.totalPage)}
                    </div> :
                    <div className='alert alert-danger'>{this.state.error}</div>}
            </div>

        return (
            <div>
                <h1>Products</h1>
                <p>List of products.</p>
                {contents}
            </div>
        );
    }
}
