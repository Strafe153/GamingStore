import React, { Component } from 'react';
import { base64ToArray } from '../modules/converter';
import { formErrorMessage } from '../modules/errorMessageFormer';

export class EditDeviceForm extends Component {
    static displayName = EditDeviceForm.name;

    constructor(props) {
        super(props);

        this.state = {
            category: this.props.location.state.category,
            price: this.props.location.state.price,
            inStock: this.props.location.state.inStock,
            companyId: this.props.location.state.companyId,
            icon: JSON.stringify(base64ToArray(this.props.location.state.icon)),
            token: sessionStorage.getItem('token')
        };

        this.updateDevice = this.updateDevice.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleFileChange = this.handleFileChange.bind(this);
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
            body: JSON.stringify({
                ...this.state,
                category: parseInt(this.state.category)
            })
        })
            .then(response => {
                if (!response.ok) {
                    return formErrorMessage(response);
                }
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
                    icon: JSON.stringify(fileByteArray)
                });
            }
        }
    }

    render() {
        return <form onSubmit={this.updateDevice}>
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
            <div className="form-group my-2">
                <label htmlFor="icon" className="form-label">Choose a picture:</label>
                <input id="icon" className="form-control" type="file" onChange={this.handleFileChange} />
            </div>

            <input className="btn btn-primary" type="submit" value="Update" />
        </form>;
    }
}