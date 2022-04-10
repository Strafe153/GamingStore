import React, { Component } from 'react';

export class AddDeviceForm extends Component {
    static displayName = AddDeviceForm.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token'),
            name: '',
            category: 0,
            price: 49.99,
            inStock: 100,
            companyId: '',
            icon: []
        };

        this.addDevice = this.addDevice.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleFileChange = this.handleFileChange.bind(this);
    }

    async addDevice(event) {
        event.preventDefault();

        await fetch('../api/devices', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            },
            body: JSON.stringify({
                ...this.state,
                category: parseInt(this.state.category),
                icon: JSON.stringify(this.state.icon)
            })
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

    handleInputChange = event => {
        const name = event.target.name;
        const value = event.target.value;

        this.setState({
            [name]: value
        });
    }

    handleFileChange = event => {
        const reader = new FileReader();
        const fileByteArray = [];

        reader.readAsArrayBuffer(event.target.files[0]);
        reader.onloadend = evt => {
            if (evt.target.readyState === FileReader.DONE) {
                const arrayBuffer = evt.target.result;
                const array = new Uint8Array(arrayBuffer);

                for (let i = 0; i < array.length; i++) {
                    fileByteArray.push(array[i]);
                }

                this.setState({
                    icon: fileByteArray
                });
            }
        }
    }

    render() {
        return <form onSubmit={this.addDevice}>
            <div className="form-group my-2">
                <label htmlFor="name" className="form-label">Name:</label>
                <input id="name" className="form-control" type="text" name="name" value={this.state.name} onChange={this.handleInputChange} />
            </div>
            <div className="form-group my-2">
                <label htmlFor="category" className="form-label">Category:</label>
                <select id="category" className="form-select" name="category" value={this.state.category} onChange={this.handleInputChange}>
                    <option value="0">Mouse</option>
                    <option value="1">Keyboard</option>
                    <option value="2">Headphones</option>
                    <option value="2">Earphones</option>
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
            <div className="form-group my-2">
                <label htmlFor="companyId" className="form-label">Company Id:</label>
                <input id="companyId" className="form-control" type="text" name="companyId" value={this.state.companyId} onChange={this.handleInputChange} />
            </div>
            <div className="form-group my-2">
                <label htmlFor="icon" className="form-label">Choose a picture:</label>
                <input id="icon" className="form-control" type="file" onChange={this.handleFileChange} />
            </div>

            <input className="btn btn-primary" type="submit" value="Create" />
        </form>;
    }
}