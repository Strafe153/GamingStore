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
        const deviceId = this.props.id;

        return <tr>
            <td className="text-center">
                <img src={ `data:image/png;base64,${this.props.icon}` } alt="device-icon" width="100" />
            </td>
            <td>{ deviceId }</td>
            <td>{ this.props.name }</td>
            <td>{ this.props.category }</td>
            <td>{ this.props.price }</td>
            <td>{ this.props.inStock }</td>
            <td>{ this.props.companyId }</td>
            <td className="text-center d-flex justify-content-around">
                <button onClick={() => this.updateDevice(deviceId)} className="btn btn-sm btn-info text-white">Edit</button>
                <button onClick={
                    async () => { 
                        await this.deleteDevice(deviceId); 
                        await this.props.getDevices(); 
                    }
                } className="btn btn-sm btn-danger">Delete</button>
            </td>
        </tr>;
    }
}