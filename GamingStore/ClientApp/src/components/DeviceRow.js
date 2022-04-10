import React, { Component } from 'react';

export class DeviceRow extends Component {
    static displayName = DeviceRow.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token')
        };
    }

    async deleteDevice(id) {
        await fetch(`../api/devices/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            }
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .catch(error => alert(error.message));
    }

    updateDevice(id) {
        window.location.href = `/devices/edit/${id}`;
    }

    render() {
        return <tr>
            <td>
                <img src={ `data:image/png;base64,${this.props.icon}` } alt="device-icon" width="100" />
            </td>
            <td>{ this.props.id }</td>
            <td>{ this.props.name }</td>
            <td>{ this.props.category }</td>
            <td>{ this.props.price }</td>
            <td>{ this.props.inStock }</td>
            <td>{ this.props.companyId }</td>
            <td className="text-center d-flex justify-content-around">
                <button onClick={() => this.updateDevice(this.props.id)} className="btn btn-sm btn-info text-white">Edit</button>
                <button onClick={
                    async () => { 
                        await this.deleteDevice(this.props.id); 
                        await this.props.getDevices(); 
                    }
                } className="btn btn-sm btn-danger">Delete</button>
            </td>
        </tr>;
    }
}