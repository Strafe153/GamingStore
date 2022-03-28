import React, { Component} from 'react';

export class UserRow extends Component {
    static displayName = UserRow.name;

    render() {
        return <tr>
            <td>{ this.props.id }</td>
            <td>{ this.props.username }</td>
            <td>{ this.props.role }</td>
        </tr>;
    }
}