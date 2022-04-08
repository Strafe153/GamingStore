import React, { Component } from 'react';

export class EditDeviceForm extends Component {
    static displayName = EditDeviceForm.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token'),
            name: '',
            category: 0,
            price: 49.99,
            inStock: 100,
            companyId: ''
        };

        this.updateDevice = this.updateDevice.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    handleInputChange = event => {
        const name = event.target.name;
        const value = event.target.value;

        this.setState({
            [name]: value
        });
    }
    async updateDevice(event) {
        event.preventDefault();

        await fetch(`../api/devices/${window.location.pathname.split('/')[3]}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .then(() => window.location.href = '/devices')
            .catch(error => alert(error.message));
    }

    render() {
        return <form onSubmit={this.updateDevice}>
            <div className="form-group my-3">
                <label htmlFor="name" className="control-label">Name:</label>
                <input id="name" className="form-control" type="text" name="name" value={this.state.name} onChange={this.handleInputChange} /> 
            </div>
            <div className="form-group my-3">
                <label htmlFor="category" className="form-label">Category:</label>
                <select id="category" className="form-select" name="category" value={this.state.category} onChange={this.handleInputChange}>
                    <option value="0">Mouse</option>
                    <option value="1">Keyboard</option>
                    <option value="2">Headphones</option>
                    <option value="3">Earphones</option>
                    <option value="4">Mat</option>
                    <option value="5">Headset</option>
                    <option value="6">Cable holder</option>
                    <option value="7">Gamepad</option>
                </select>
            </div>
            <div className="form-group my-2">
                <label htmlFor="price" className="form-label">Price:</label>
                <input id="price" className="form-control" type="number" name="price" value={this.state.price} onChange={this.handleInputChange} />
            </div>
            <div className="form-group my-2">
                <label htmlFor="in-stock" className="form-label">In stock:</label>
                <input id="in-stock" className="form-control" type="number" name="inStock" value={this.state.inStock} onChange={this.handleInputChange} /> 
            </div>

            <input className="btn btn-primary" type="submit" value="Update" />
        </form>;
    }
}