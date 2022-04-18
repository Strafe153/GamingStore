import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';
import { formErrorMessage } from '../modules/errorMessageFormer';

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
                    return formErrorMessage(response);
                }
            })
            .catch(error => alert(error.message));
    }

    updateDevice(id) {
        window.location.href = `/devices/edit/${id}`;
    }

    render() {
        const id = this.props.id;
        const name = this.props.name;
        const category = this.props.category;
        const price = this.props.price;
        const inStock = this.props.inStock;
        const companyId = this.props.companyId;
        const icon = this.props.icon;

        return <tr>
            <td className="text-center">
                <img src={ `data:image/png;base64,${icon}` } alt="device-icon" width="100" />
            </td>
            <td>{ id }</td>
            <td>{ name }</td>
            <td>{ category }</td>
            <td>{ price }</td>
            <td>{ inStock }</td>
            <td>{ companyId }</td>
            <td className="text-center d-flex justify-content-around">
                <NavLink className="btn btn-sm btn-info text-white" to={{
                    pathname: `/devices/edit/${id}`,
                    state: {
                        category: category,
                        price: price,
                        inStock: inStock,
                        companyId: companyId,
                        icon: icon
                    }
                }}>Edit</NavLink>
                <button onClick={
                    async () => { 
                        await this.deleteDevice(id); 
                        await this.props.getDevices(); 
                    }
                } className="btn btn-sm btn-danger">Delete</button>
            </td>
        </tr>;
    }
}