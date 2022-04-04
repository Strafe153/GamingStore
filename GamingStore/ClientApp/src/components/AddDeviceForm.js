import React, { Component } from 'react';

export class AddDeviceForm extends Component {
    static displayName = AddDeviceForm.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token'),
            name: '',
            category: 0,
            price: 50,
            companyId: ''
        };

        this.addDevice = this.addDevice.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    async addDevice(event) {
        event.preventDefault();
        console.log(typeof(this.state.category));
        console.log(this.state.category);

        await fetch('../api/devices', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                return response.text().then(error => { throw new Error(error) });
            })
            .then(() => window.location.href = '/devices')
            .catch(error => alert(error.message));
    }

    handleInputChange(event) {
        const name = event.target.name;
        const value = event.target.value;

        this.setState({
            [name]: value
        });
    }

    handleRangeInut(event) {
        const outputField = document.querySelector("output");
        outputField.value = event.target.value;
    }

    render() {
        return <form onSubmit={this.addDevice}>
            <div className="form-group my-3">
                <label htmlFor="name" className="form-label">Name:</label>
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
            <div className="form-group my-3">
                <label htmlFor="price" className="form-label">Price:</label>
                <input id="price" className="form-range" type="range" min="1" max="1000" name="price" value={this.state.price} onChange={this.handleInputChange} onInput={this.handleRangeInut} />
                <output className="d-flex justify-content-center">50</output>
            </div>
            <div className="form-group my-3">
                <label htmlFor="companyId" className="form-label">Company Id:</label>
                <input id="companyId" className="form-control" type="text" name="companyId" value={this.state.companyId} onChange={this.handleInputChange} />
            </div>

            <input className="btn btn-primary" type="submit" value="Create" />
        </form>;
    }
}